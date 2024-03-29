﻿using CodeBase.Animation;
using CodeBase.Effects;
using CodeBase.Mobs;
using CodeBase.ObjectBased;
using CodeBase.Service;
using CodeBase.UI;
using CodeBase.Utils;
using DG.Tweening;
using Scripts;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;
using static CodeBase.Utils.Enums;

namespace CodeBase.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Variables
        [Header("Storages")]
        [SerializeField] private PlayerStorage playerStorage;
        [SerializeField] private EnemyStorage enemyStorage;
        [SerializeField] private CollectableStorage collectableStorage;

        [Header("Shields")]
        [SerializeField] private Shield electroShield;
        [field: SerializeField] public float ElectroShieldActiveDuration { get; private set; }

        [Header("Components")]
        [SerializeField] private PlayerAnimationController playerAnimationController;
        [SerializeField] private WeaponController weaponController;

        [Space]
        [SerializeField] private PopUp popUp;
        [SerializeField] private SpriteAnimator levelUpEffect;
        [SerializeField] private ColouredEffect pickUpEffect;
        [SerializeField] private GameObject body;
        [SerializeField] private Collider2D playerCollider;
        [SerializeField] private ParticleType explosionEffect;
        [SerializeField] private float explosionAdditionalScale;
        [SerializeField] private Rigidbody2D playerBody;
        [SerializeField] private SpriteRenderer skinRenderer;
        [SerializeField] private float forceOnEnemyCollision;
        [SerializeField] private float minPercentOfHealthToBlink;
        [SerializeField] private Color playerHitColor = Color.red;

        private AudioComponent _audio;
        private Color defaultColor;
        private Coroutine newLifeCoroutine;
        private Coroutine gameOverCoroutine;
        private Sequence playerCollisionBehaviour;
        private bool isChangedColor;
        private ParticlePool particlePool;
        private TouchController touchController;
        private UserInterface userInterface;
        #endregion

        [Inject]
        private void Construct(ParticlePool pool, TouchController touch, UserInterface ui)
        {
            particlePool = pool;
            touchController = touch;
            userInterface = ui;
        }

        private void Awake()
        {
            _audio = FindObjectOfType<AudioComponent>();
        }

        private void OnEnable()
        {
            EventObserver.OnLevelLoaded += EnableTouchControls;
            EventObserver.OnGameRestarted += StartNewGame;
            EventObserver.OnPlayerCollision += ForceBackPlayer;
            EventObserver.OnLevelChanged += SpawnLvlPopup;
            EventObserver.OnCollectableGot += PerformPickUpEffect;
        }

        private void OnDisable()
        {
            EventObserver.OnLevelLoaded -= EnableTouchControls;
            EventObserver.OnGameRestarted -= StartNewGame;
            EventObserver.OnPlayerCollision -= ForceBackPlayer;
            EventObserver.OnLevelChanged -= SpawnLvlPopup;
            EventObserver.OnCollectableGot -= PerformPickUpEffect;
        }

        private void Start()
        {
            defaultColor = skinRenderer.color;
            transform.position = playerStorage.PlayerData.DefaultPlayerPosition;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.EnemyProjectile) && !electroShield.IsActive)
            {
                var projectile = Dictionaries.EnemyProjectiles.FirstOrDefault(p => p.Key == collision.gameObject.transform);
                playerStorage.PlayerData.ModifyHealth(-projectile.Value.Damage);

                SpawnSpark(collision.gameObject.transform.position);
                CheckBehaviourDueToDamageTaken();
            }
        }

        private void EnableTouchControls() => touchController.enabled = true;

        private void SpawnLvlPopup()
        {
            levelUpEffect.gameObject.SetActive(true);
            popUp.Spawn(transform, $"lvl up", Color.yellow);
        }

        private void PerformPickUpEffect(CollectableType type)
        {
            var collectableData = collectableStorage.GetCollectableInfo(type);

            pickUpEffect.SetColor(collectableData.Color);
            pickUpEffect.gameObject.SetActive(true);

            popUp.Spawn(transform, $"{collectableData.Name}", collectableData.Color);

            switch (type)
            {
                case CollectableType.PowerUp:
                    userInterface.RefreshPowerSpotInfo(3);//TODO
                    break;

                case CollectableType.OneUp:
                    playerStorage.PlayerData.ModifyTries(1);
                    break;

                case CollectableType.MachineGun:
                case CollectableType.Laser:
                case CollectableType.Blaster:
                case CollectableType.Energy:
                    userInterface.RefreshWeaponSpotInfo(type);
                    break;
            }
        }

        private void CheckBehaviourDueToDamageTaken()
        {
            var minHealthEdge = (playerStorage.PlayerData.Health / 100f) * minPercentOfHealthToBlink;
            if (playerStorage.PlayerData.CurrentHealth <= minHealthEdge && playerStorage.PlayerData.CurrentHealth > 0f)
            {
                playerAnimationController.EnableCriticalDamageVisual(true);
                popUp.Spawn(transform, "danger", Color.red);
            }

            if (playerStorage.PlayerData.IsDead)
            {
                DestroyPlayer();
                EventObserver.OnPlayerDied?.Invoke();

                if (playerStorage.PlayerData.CurrentTries > 0 && newLifeCoroutine == null)
                    newLifeCoroutine = StartCoroutine(StartNewLife());
                else if (playerStorage.PlayerData.CurrentTries <= 0 && gameOverCoroutine == null)
                    gameOverCoroutine = StartCoroutine(GameOver());
            }

            ChangeBodyColor();

            EventObserver.OnShakeCamera?.Invoke(0.2f, 0.25f);
            EventObserver.OnPlayerHit?.Invoke();
        }

        private void ChangeBodyColor()
        {
            if (!isChangedColor)
            {
                isChangedColor = true;

                playerCollisionBehaviour = DOTween.Sequence().SetAutoKill(true);
                playerCollisionBehaviour.Append(skinRenderer.DOColor(playerHitColor, 0.1f))
                                        .Append(skinRenderer.DOColor(defaultColor, 0.1f))
                                        .OnComplete(() => isChangedColor = false);
            }
        }

        private IEnumerator GameOver()
        {
            DestroyPlayer();
            yield return new WaitForSeconds(3f);
            EventObserver.OnGameOver?.Invoke();
            gameOverCoroutine = null;
        }

        private IEnumerator StartNewLife()
        {
            DestroyPlayer();

            yield return new WaitForSeconds(3f);

            transform.position = playerStorage.PlayerData.DefaultPlayerPosition;
            electroShield.gameObject.SetActive(true);

            yield return new WaitForSeconds(0.5f);

            touchController.enabled = true;

            body.SetActive(true);
            playerCollider.enabled = true;

            playerAnimationController.EnableStarterFlames(true);
            playerStorage.PlayerData.RevivePlayer();
            newLifeCoroutine = null;
        }

        private void SpawnSpark(Vector3 projectilePosition)
        {
            var newEffect = particlePool.GetFreeObject(ParticleType.SparkHit);
            newEffect.gameObject.SetActive(false);
            newEffect.transform.position = new Vector3(projectilePosition.x, projectilePosition.y - 1f, projectilePosition.z);
            newEffect.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            newEffect.SetBusyState(true);
        }

        private void ForceBackPlayer(Vector3 asteroidPosition)
        {
            var force = asteroidPosition - transform.position;
            playerBody.AddForce(-force.normalized * (forceOnEnemyCollision * 100f));
            playerStorage.PlayerData.ModifyHealth(-enemyStorage.DamageOnCollision);

            CheckBehaviourDueToDamageTaken();
        }

        private void DestroyPlayer()
        {
            touchController.enabled = false;
            weaponController.StartShooting(false);
            playerAnimationController.EnableCriticalDamageVisual(false);
            playerAnimationController.EnableStarterFlames(false);

            var newEffect = particlePool.GetFreeObject(explosionEffect);
            newEffect.gameObject.SetActive(false);
            newEffect.transform.position = transform.position;
            newEffect.transform.localScale = new Vector3(transform.localScale.x + explosionAdditionalScale, transform.localScale.y + explosionAdditionalScale, 1f);
            newEffect.SetBusyState(true);

            body.SetActive(false);
            playerCollider.enabled = false;
        }

        private void StartNewGame()
        {
            playerStorage.PlayerData.StartNewGame();
            playerAnimationController.EnableStarterFlames(true);

            body.SetActive(true);
            playerCollider.enabled = true;
            transform.position = playerStorage.PlayerData.DefaultPlayerPosition;
        }
    }
}

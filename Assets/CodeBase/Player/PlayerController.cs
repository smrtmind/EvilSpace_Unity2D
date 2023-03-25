using CodeBase.Animation;
using CodeBase.Mobs;
using CodeBase.ObjectBased;
using CodeBase.Service;
using CodeBase.UI;
using CodeBase.Utils;
using DG.Tweening;
using Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;
        [SerializeField] private PlayerStorage playerStorage;
        [SerializeField] private EnemyStorage enemyStorage;

        [Header("Shields")]
        [SerializeField] private Shield electroShield;
        [SerializeField] private Shield safeZoneShield;

        [SerializeField] private PlayerAnimationController playerAnimationController;

        [Space]
        [SerializeField] private PopUp popUp;
        [SerializeField] private GameObject body;
        [SerializeField] private Collider2D playerCollider;
        [SerializeField] private ParticleType explosionEffect;
        [SerializeField] private float explosionAdditionalScale;
        [SerializeField] private Rigidbody2D playerBody;
        [SerializeField] private SpriteRenderer skinRenderer;
        [SerializeField] private GameObject _leftWingDamage;
        [SerializeField] private GameObject _rightWingDamage;
        [SerializeField] private GameObject _bodyDamage;
        [SerializeField] private GameObject _safeZone;
        [SerializeField] private GameObject _safeZoneEffect;
        [SerializeField] private float forceOnEnemyCollision;
        [SerializeField] private List<string> tagsToReact;

        public static Action OnPlayerDied;
        public static Action<Vector3> OnPlayerCollision;

        private AudioComponent _audio;
        private Tween skinColorTween;
        private Color defaultColor;
        private Coroutine newLifeCoroutine;
        private Coroutine gameOverCoroutine;
        private Tween playerCollisionTween;
        private bool isRotating;

        private void Awake()
        {
            _audio = FindObjectOfType<AudioComponent>();
        }

        private void OnEnable()
        {
            defaultColor = skinRenderer.color;

            UserInterface.OnLevelLoaded += InitPlayer;
            UserInterface.OnGameRestarted += StartNewGame;
            OnPlayerCollision += ForceBackPlayer;

        }

        private void OnDisable()
        {
            UserInterface.OnLevelLoaded -= InitPlayer;
            UserInterface.OnGameRestarted -= StartNewGame;
            OnPlayerCollision -= ForceBackPlayer;
        }

        private void InitPlayer()
        {
            transform.position = playerStorage.ConcretePlayer.DefaultPlayerPosition;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (string tag in tagsToReact)
            {
                if (collision.gameObject.tag.Equals(tag))
                {
                    var damage = GetCurrentEnemyDamage(collision);
                    playerStorage.ConcretePlayer.ModifyHealth(-damage);

                    popUp.SetCurrentData(transform, $"-{damage}", "red");
                    popUp.SpawnPopUp();

                    skinColorTween?.Kill();
                    skinColorTween = skinRenderer.DOColor(Color.red, 0.1f).OnComplete(() => skinRenderer.color = defaultColor);

                    SpawnSpark(collision.gameObject.transform.position);
                    CameraShaker.OnShakeCamera?.Invoke();

                    if (playerStorage.ConcretePlayer.IsDead)
                    {
                        if (playerStorage.ConcretePlayer.CurrentTries > 0)
                            newLifeCoroutine = StartCoroutine(StartNewLife());
                        else
                            gameOverCoroutine = StartCoroutine(GameOver());

                        OnPlayerDied?.Invoke();
                    }
                }
            }
        }

        private float GetCurrentEnemyDamage(Collider2D collision)
        {
            float damage = 0f;

            switch (collision.gameObject.tag)
            {
                case "EnemyProjectile":
                    var projectile = Dictionaries.Projectiles.FirstOrDefault(p => p.Key == collision.gameObject.transform);                   
                    damage = projectile.Value.Damage;
                    break;

                case "Enemy":
                    damage = enemyStorage.DamageOnCollision;
                    break;
            }

            return damage;
        }

        private IEnumerator GameOver()
        {
            DestroyPlayer();
            yield return new WaitForSeconds(3f);
            UserInterface.OnGameOver?.Invoke();
        }

        private IEnumerator StartNewLife()
        {
            DestroyPlayer();

            yield return new WaitForSeconds(3f);

            body.SetActive(true);
            playerCollider.enabled = true;
            transform.position = playerStorage.ConcretePlayer.DefaultPlayerPosition;

            playerStorage.ConcretePlayer.RevivePlayer();
        }

        private void SpawnSpark(Vector3 projectilePosition)
        {
            var newEffect = dependencyContainer.ParticlePool.GetFreeObject(ParticleType.SparksHit);
            newEffect.gameObject.SetActive(false);
            newEffect.transform.position = new Vector3(projectilePosition.x, projectilePosition.y - 1f, projectilePosition.z);
            newEffect.transform.localScale = new Vector3(0.75f, 0.75f, 1f);
            newEffect.SetBusyState(true);
        }

        private void ForceBackPlayer(Vector3 asteroidPosition)
        {
            var force = asteroidPosition - transform.position;
            playerBody.AddForce(-force.normalized * forceOnEnemyCollision);

            if (!isRotating)
            {
                isRotating = true;
                playerCollisionTween?.Kill();
                playerCollisionTween = transform.DORotate(new Vector3(0f, 0f, 360f), 1f, RotateMode.FastBeyond360).OnComplete(() => isRotating = false);
            }
        }

        private void DestroyPlayer()
        {
            var newEffect = dependencyContainer.ParticlePool.GetFreeObject(explosionEffect);
            newEffect.gameObject.SetActive(false);
            newEffect.transform.position = transform.position;
            newEffect.transform.localScale = new Vector3(transform.localScale.x + explosionAdditionalScale, transform.localScale.y + explosionAdditionalScale, 1f);
            newEffect.SetBusyState(true);

            body.SetActive(false);
            playerCollider.enabled = false;
        }

        private void StartNewGame()
        {
            playerStorage.ConcretePlayer.StartNewGame();

            body.SetActive(true);
            playerCollider.enabled = true;
            transform.position = playerStorage.ConcretePlayer.DefaultPlayerPosition;
        }
    }
}

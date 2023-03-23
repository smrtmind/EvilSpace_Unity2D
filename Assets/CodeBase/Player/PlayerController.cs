using CodeBase.Animation;
using CodeBase.Mobs;
using CodeBase.ObjectBased;
using CodeBase.Service;
using CodeBase.UI;
using CodeBase.Utils;
using DG.Tweening;
using Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;
        [SerializeField] private PlayerStorage playerStorage;

        [Header("Shields")]
        [SerializeField] private Shield electroShield;
        [SerializeField] private Shield safeZoneShield;

        [SerializeField] private PlayerAnimationController playerAnimationController;

        [Space]
        [SerializeField] private ParticleType explosionEffect;
        [SerializeField] private Rigidbody2D playerBody;
        [SerializeField] private SpriteRenderer skinRenderer;
        [SerializeField] private GameObject _leftWingDamage;
        [SerializeField] private GameObject _rightWingDamage;
        [SerializeField] private GameObject _bodyDamage;
        [SerializeField] private GameObject _safeZone;
        [SerializeField] private GameObject _safeZoneEffect;
        [SerializeField] private float forceOnEnemyCollision;
        [SerializeField] private List<string> tagsToReact;

        private static readonly int LowHpKey = Animator.StringToHash("lowHp");
        private static readonly int LeftTurnKey = Animator.StringToHash("left-turn");
        private static readonly int RightTurnKey = Animator.StringToHash("right-turn");
        private static readonly int HitKey = Animator.StringToHash("is-hit");
        private static readonly int HitLeftKey = Animator.StringToHash("is-hitLeft");
        private static readonly int HitRightKey = Animator.StringToHash("is-hitRight");

        public static Action OnPlayerDamaged;

        public bool IsDead { get; set; }
        public bool secondWeapon { get; set; }
        public bool thirdWeapon { get; set; }

        private CameraShaker _cameraShaker;
        private AudioComponent _audio;
        private Tween skinColorTween;
        private Color defaultColor;

        private void Awake()
        {
            _audio = FindObjectOfType<AudioComponent>();
            _cameraShaker = FindObjectOfType<CameraShaker>();
        }

        private void OnEnable()
        {
            defaultColor = skinRenderer.color;

            //OnPlayerDamaged += PlayerDamaged;
            UserInterface.OnLevelLoaded += InitPlayer;
            Asteroid.OnPlayerCollision += ForceBackPlayer;
        }

        private void OnDisable()
        {
            //OnPlayerDamaged -= PlayerDamaged;
            UserInterface.OnLevelLoaded -= InitPlayer;
            Asteroid.OnPlayerCollision -= ForceBackPlayer;
        }

        private void InitPlayer()
        {
            transform.position = playerStorage.ConcretePlayer.DefaultPlayerPosition;
        }

        //private void FixedUpdate()
        //{
        //    if (playerInput.LeftTurn)
        //    {
        //        playerBody.velocity = new Vector2(playerInput.Direction * (playerStorage.ConcretePlayer.MovementSpeed * 100f) * Time.deltaTime, 0f);

        //        SetAnimationStatus(true, LeftTurnKey);
        //        SetAnimationStatus(false, RightTurnKey);

        //        SetObjectStatus(false, _idleStarterFlameFirst);
        //        SetObjectStatus(true, _idleStarterFlameSecond);
        //    }
        //    else if (playerInput.RightTurn)
        //    {
        //        playerBody.velocity = new Vector2(playerInput.Direction * (playerStorage.ConcretePlayer.MovementSpeed * 100f) * Time.deltaTime, 0f);

        //        SetAnimationStatus(false, LeftTurnKey);
        //        SetAnimationStatus(true, RightTurnKey);

        //        SetObjectStatus(true, _idleStarterFlameFirst);
        //        SetObjectStatus(false, _idleStarterFlameSecond);
        //    }
        //    else
        //    {
        //        SetAnimationStatus(false, LeftTurnKey, RightTurnKey);
        //        SetObjectStatus(false, _idleStarterFlameFirst, _idleStarterFlameSecond);
        //    }
        //}

        private void SetObjectStatus(bool state, params GameObject[] gos)
        {
            foreach (var go in gos)
            {
                go.SetActive(state);
            }
        }

        private void SetAnimationStatus(bool state, params int[] animations)
        {
            foreach (var animation in animations)
            {
                //playerAnimator.SetBool(animation, state);
            }
        }

        public void RemoveVisualDamage()
        {
            //playerAnimator.SetBool(LowHpKey, false);
            SetObjectStatus(false, _leftWingDamage, _rightWingDamage, _bodyDamage);
        }

        private async void NewLife()
        {
            playerBody.gameObject.SetActive(false);
            //safeZoneShield.gameObject.SetActive(true);

            await Task.Delay(3000);

            //safeZoneShield.gameObject.SetActive(false);
            playerStorage.ConcretePlayer.PlayerIsDead(false);
            playerBody.gameObject.SetActive(true);
            electroShield.Activate();
            playerStorage.ConcretePlayer.ModifyHealth(playerStorage.ConcretePlayer.MaxHealth);
        }

        private void PlayerDamaged()
        {
            if (playerStorage.ConcretePlayer.IsDead)
            {
                playerBody.gameObject.SetActive(false);
            }

            var session = FindObjectOfType<GameSession>();

            //Instantiate(_hitParticles, other.GetContact(0).point, Quaternion.identity);
            //_cameraShaker.ShakeCamera();

            var projectile = FindObjectOfType<Projectile>();
            if (projectile)
            {
                //FindObjectOfType<CameraShaker>().RestoreValues();
            }

            if (playerStorage.ConcretePlayer.CurrentHealth == 3)
            {
                SetObjectStatus(true, _leftWingDamage);
            }

            if (playerStorage.ConcretePlayer.CurrentHealth == 2)
            {
                SetObjectStatus(true, _rightWingDamage);
            }

            if (playerStorage.ConcretePlayer.CurrentHealth == 1)
            {
                SetObjectStatus(true, _bodyDamage);
                SetAnimationStatus(true, LowHpKey);
            }

            if (playerStorage.ConcretePlayer.IsDead)
            {
                if (playerStorage.ConcretePlayer.CurrentTries > 0)
                {
                    SetObjectStatus(false, _leftWingDamage, _rightWingDamage, _bodyDamage, gameObject);
                    //_safeZone.GetComponent<Collider2D>().enabled = true;
                    //_safeZoneEffect.SetActive(true);
                    //_safeZone.GetComponent<TimerComponent>().SetTimer(0);






                    NewLife();






                    //_timers.SetTimerByName("new life");

                    transform.position = Vector3.zero;
                    transform.rotation = Quaternion.identity;
                }
                else
                {
                    SetObjectStatus(false, gameObject);
                    _audio.StopMainSource();
                    _audio.Stop();
                    //_timers.SetTimerByName("game over");
                }
            }

            //if (playerInput.LeftTurn)
            //{
            //    playerAnimator.SetTrigger(HitLeftKey);
            //}
            //if (playerInput.RightTurn)
            //{
            //    playerAnimator.SetTrigger(HitRightKey);
            //}
            //else
            //    playerAnimator.SetTrigger(HitKey);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            foreach (string tag in tagsToReact)
            {
                if (collision.gameObject.tag.Equals(tag))
                {
                    var projectile = Dictionaries.Projectiles.FirstOrDefault(p => p.Key == collision.gameObject.transform);
                    //ModifyHealth(-projectile.Value.WeaponData.Damage);

                    skinColorTween?.Kill();
                    skinColorTween = skinRenderer.DOColor(Color.red, 0.1f).OnComplete(() => skinRenderer.color = defaultColor);

                    SpawnSpark(collision.gameObject.transform.position);

                    CameraShaker.OnShakeCamera?.Invoke();
                }
            }
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
        }
    }
}

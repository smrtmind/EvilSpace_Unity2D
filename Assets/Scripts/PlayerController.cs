using UnityEngine;

namespace Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 150;
        [SerializeField] private float _burstSpeed = 1;
        [SerializeField] private GameObject _idleStarterFlameFirst;
        [SerializeField] private GameObject _idleStarterFlameSecond;
        [SerializeField] private GameObject _leftWingDamage;
        [SerializeField] private GameObject _rightWingDamage;
        [SerializeField] private GameObject _bodyDamage;
        [SerializeField] private float _damageVelocity;

        [Space]
        [Header("Weapon")]
        [SerializeField] private Projectile _machineGun;
        [SerializeField] private Cooldown _machineGunCooldown;
        [SerializeField] private Transform _machineGunSpawnPosition;

        [Space]
        [SerializeField] private Projectile _twinLaser;
        [SerializeField] private Cooldown _twinLaserCooldown;
        [SerializeField] private Transform _twinLaserSpawnPosition;

        //[Space]
        //[SerializeField] private Projectile _machineGun;
        //[SerializeField] private Cooldown _machineGunCooldown;
        //[SerializeField] private Transform _machineGunSpawnPosition;

        [Space]
        [Header("Sounds")]
        [SerializeField] private AudioClip _shipHit;

        private readonly int DangerHpKey = Animator.StringToHash("dangerHp");
        private readonly int LeftStarterKey = Animator.StringToHash("left-turn");
        private readonly int RightStarterKey = Animator.StringToHash("right-turn");
        private readonly int HitKey = Animator.StringToHash("is-hit");
        private readonly int HitLeftKey = Animator.StringToHash("is-hitLeft");
        private readonly int HitRightKey = Animator.StringToHash("is-hitRight");

        public float burst { get; set; }
        public bool firstWeapon { get; set; }
        public bool secondWeapon { get; set; }

        private HealthComponent _health;
        private Animator _animator;
        private Rigidbody2D _body;
        private AudioSource _audio;
        private bool _isLeftPressed;
        private bool _isRightPressed;
        private bool _isMovingForward;

        private void Start()
        {
            _health = GetComponent<HealthComponent>();
            _body = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _audio = FindObjectOfType<AudioSource>();
        }

        private void Update()
        {
            if (firstWeapon && _machineGunCooldown.IsReady && !secondWeapon)
            {
                var projectile = Instantiate(_machineGun, _machineGunSpawnPosition.position, transform.rotation);
                projectile.Launch(_body.velocity, transform.up);
                _machineGunCooldown.Reset();
            }
            if (secondWeapon && _twinLaserCooldown.IsReady && !firstWeapon)
            {
                var projectile = Instantiate(_twinLaser, _twinLaserSpawnPosition.position, transform.rotation);
                projectile.Launch(_body.velocity, transform.up);
                _twinLaserCooldown.Reset();
            }
        }

        private void FixedUpdate()
        {
            _isLeftPressed = Input.GetKey(KeyCode.LeftArrow);
            _isRightPressed = Input.GetKey(KeyCode.RightArrow);
            _isMovingForward = burst > 0;

            if (_isLeftPressed)
            {
                _body.angularVelocity = 1 * _rotationSpeed;

                PlayAnimation(LeftStarterKey);
                ActivateObject(_idleStarterFlameSecond);
            }
            else if (_isRightPressed)
            {
                _body.angularVelocity = -1 * _rotationSpeed;

                PlayAnimation(RightStarterKey);
                ActivateObject(_idleStarterFlameFirst);
            }
            else
            {
                PlayAnimation(disableAll: true);
                ActivateObject(disableAll: true);
            }

            if (_isMovingForward)
            {
                Vector2 burstDelta = transform.up * _burstSpeed;
                _body.velocity += burstDelta;

                ActivateObject(disableAll: false);
            }
        }

        private void PlayAnimation(int animation = default, bool disableAll = false)
        {
            _animator.SetBool(LeftStarterKey, false);
            _animator.SetBool(RightStarterKey, false);

            if (!disableAll)
                _animator.SetBool(animation, true);
        }

        private void ActivateObject(GameObject go = null, bool disableAll = false)
        {
            SetGeneralState(false);

            if (go != null)
                go.SetActive(true);
            else
                SetGeneralState(disableAll ? false : true);

            void SetGeneralState(bool state)
            {
                _idleStarterFlameFirst.SetActive(state);
                _idleStarterFlameSecond.SetActive(state);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (_health.Health == 3)
                _leftWingDamage.SetActive(true);
            if (_health.Health == 2)
                _rightWingDamage.SetActive(true);
            if (_health.Health == 1)
            {
                _bodyDamage.SetActive(true);
                _animator.SetBool(DangerHpKey, true);
            }
                

            var asteroid = other.gameObject.GetComponent<Asteroid>();
            _body.velocity = new Vector2(_body.velocity.x, _damageVelocity);
            _audio.PlayOneShot(_shipHit);

            if (_isLeftPressed)
            {
                _animator.SetTrigger(HitLeftKey);
                if (_isMovingForward) return;
            }
            if (_isRightPressed)
            {
                _animator.SetTrigger(HitRightKey);
                if (_isMovingForward) return;
            }
            else
                _animator.SetTrigger(HitKey);
        }
    }
}

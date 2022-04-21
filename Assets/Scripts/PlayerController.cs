using UnityEngine;

namespace Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 150f;
        [SerializeField] private float _burstSpeed = 1f;
        [SerializeField] private GameObject _idleStarterFlameFirst;
        [SerializeField] private GameObject _idleStarterFlameSecond;
        [SerializeField] private GameObject _leftWingDamage;
        [SerializeField] private GameObject _rightWingDamage;
        [SerializeField] private GameObject _bodyDamage;
        [SerializeField] private float _damageVelocity;
        [SerializeField] private GameObject _hitParticles;
        [SerializeField] private TimerComponent _timerToContinue;
        [SerializeField] private TimerComponent _timerToGameOver;
        [SerializeField] private AudioSource _mainTheme;

        [Space]
        [Header("Sounds")]
        [SerializeField] private AudioClip _shipHit;

        private readonly int LowHpKey = Animator.StringToHash("lowHp");
        private readonly int LeftTurnKey = Animator.StringToHash("left-turn");
        private readonly int RightTurnKey = Animator.StringToHash("right-turn");
        private readonly int HitKey = Animator.StringToHash("is-hit");
        private readonly int HitLeftKey = Animator.StringToHash("is-hitLeft");
        private readonly int HitRightKey = Animator.StringToHash("is-hitRight");

        public float burst { get; set; }
        public bool firstWeapon { get; set; }
        public bool secondWeapon { get; set; }
        public bool thirdWeapon { get; set; }

        private HealthComponent _health;
        private Animator _animator;
        private Rigidbody2D _body;
        private AudioSource _audio;
        private bool _isLeftPressed;
        private bool _isRightPressed;
        private bool _isMovingForward;
        private CameraShaker _cameraShaker;

        private void Start()
        {
            _cameraShaker = FindObjectOfType<CameraShaker>();
            _health = GetComponent<HealthComponent>();
            _body = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _audio = FindObjectOfType<AudioSource>();
        }

        private void FixedUpdate()
        {
            _isLeftPressed = Input.GetKey(KeyCode.LeftArrow);
            _isRightPressed = Input.GetKey(KeyCode.RightArrow);
            _isMovingForward = burst > 0;

            if (_isLeftPressed)
            {
                _body.angularVelocity = 1 * _rotationSpeed;

                SetAnimationStatus(true, LeftTurnKey);
                SetAnimationStatus(false, RightTurnKey);

                SetObjectStatus(false, _idleStarterFlameFirst);
                SetObjectStatus(true, _idleStarterFlameSecond);
            }
            else if (_isRightPressed)
            {
                _body.angularVelocity = -1 * _rotationSpeed;

                SetAnimationStatus(false, LeftTurnKey);
                SetAnimationStatus(true, RightTurnKey);

                SetObjectStatus(true, _idleStarterFlameFirst);
                SetObjectStatus(false, _idleStarterFlameSecond);
            }
            else
            {
                SetAnimationStatus(false, LeftTurnKey, RightTurnKey);
                SetObjectStatus(false, _idleStarterFlameFirst, _idleStarterFlameSecond);
            }

            if (_isMovingForward)
            {
                Vector2 burstDelta = transform.up * _burstSpeed;
                _body.velocity += burstDelta;

                SetObjectStatus(true, _idleStarterFlameFirst, _idleStarterFlameSecond);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var session = FindObjectOfType<GameSession>();
            _audio.PlayOneShot(_shipHit);

            Instantiate(_hitParticles, other.GetContact(0).point, Quaternion.identity);
            _cameraShaker.ShakeCamera();

            var projectile = FindObjectOfType<Projectile>();
            if (projectile)
            {
                FindObjectOfType<CameraShaker>().RestoreValues();
            }

            if (!projectile)
            {
                _body.velocity = new Vector2(_body.velocity.x, _damageVelocity);
            }

            if (_health.Health == 3)
            {
                SetObjectStatus(true, _leftWingDamage);
            }
                
            if (_health.Health == 2)
            {
                SetObjectStatus(true, _rightWingDamage);
            }
                
            if (_health.Health == 1)
            {
                SetObjectStatus(true, _bodyDamage);
                SetAnimationStatus(true, LowHpKey);
            }

            if (_health.Health <= 0)
            {
                if (session.Tries > 0)
                {
                    SetObjectStatus(false, _leftWingDamage, _rightWingDamage, _bodyDamage, gameObject);
                    _timerToContinue.SetTimer(0);

                    transform.position = Vector3.zero;
                    transform.rotation = Quaternion.identity;
                }
                else
                {
                    SetObjectStatus(false, gameObject);
                    _mainTheme.Stop();
                    _timerToGameOver.SetTimer(0);
                }
            }

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
                _animator.SetBool(animation, state);
            }
        }

        public void RemoveVisualDamage()
        {
            _animator.SetBool(LowHpKey, false);
            SetObjectStatus(false, _leftWingDamage, _rightWingDamage, _bodyDamage);
        }
    }
}

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
        [SerializeField] private GameObject _hitParticles;
        [SerializeField] private TimerComponent _timerToContinue;
        [SerializeField] private TimerComponent _timerToGameOver;
        [SerializeField] private AudioSource _mainTheme;
        [SerializeField] public SpawnComponent _levelUpEffect;
        [SerializeField] private GameObject _safeZone;
        [SerializeField] private GameObject _safeZoneEffect;

        private static readonly int LowHpKey = Animator.StringToHash("lowHp");
        private static readonly int LeftTurnKey = Animator.StringToHash("left-turn");
        private static readonly int RightTurnKey = Animator.StringToHash("right-turn");
        private static readonly int HitKey = Animator.StringToHash("is-hit");
        private static readonly int HitLeftKey = Animator.StringToHash("is-hitLeft");
        private static readonly int HitRightKey = Animator.StringToHash("is-hitRight");

        // <---------------------------------- FOR MOBILE BUILD start
        //private Joystick _joystick;
        // <---------------------------------- FOR MOBILE BUILD end

        // <---------------------------------- FOR PC BUILD start
        public float burst { get; set; }
        // <---------------------------------- FOR PC BUILD end

        public bool leftTurn { get; set; }
        public bool rightTurn { get; set; }
        public bool firstWeapon { get; set; }
        public bool secondWeapon { get; set; }
        public bool thirdWeapon { get; set; }

        private HealthComponent _health;
        private Animator _animator;
        private Rigidbody2D _playerBody;
        private bool _isMovingForward;
        private CameraShaker _cameraShaker;
        private bool _isDead;
        private Collider2D _playerCollider;

        public bool IsDead => _isDead;

        private void Start()
        {
            _cameraShaker = FindObjectOfType<CameraShaker>();
            _health = GetComponent<HealthComponent>();
            _playerBody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            _playerCollider = GetComponent<Collider2D>();

            // <---------------------------------- FOR MOBILE BUILD start
            //_joystick = FindObjectOfType<Joystick>();
            // <---------------------------------- FOR MOBILE BUILD end
        }

        private void FixedUpdate()
        {
            // <---------------------------------- FOR PC BUILD start
            _isMovingForward = burst > 0;
            // <---------------------------------- FOR PC BUILD end

            // <---------------------------------- FOR MOBILE BUILD start
            //leftTurn = _joystick.Horizontal <= -0.5f;
            //rightTurn = _joystick.Horizontal >= 0.5f;
            //_isMovingForward = _joystick.Vertical <= -0.2f || _joystick.Vertical >= 0.2f;
            // <---------------------------------- FOR MOBILE BUILD end

            if (leftTurn)
            {
                _playerBody.angularVelocity = 1 * _rotationSpeed;

                SetAnimationStatus(true, LeftTurnKey);
                SetAnimationStatus(false, RightTurnKey);

                SetObjectStatus(false, _idleStarterFlameFirst);
                SetObjectStatus(true, _idleStarterFlameSecond);
            }
            else if (rightTurn)
            {
                _playerBody.angularVelocity = -1 * _rotationSpeed;

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
                _playerBody.velocity += burstDelta;

                SetObjectStatus(true, _idleStarterFlameFirst, _idleStarterFlameSecond);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var session = FindObjectOfType<GameSession>();

            Instantiate(_hitParticles, other.GetContact(0).point, Quaternion.identity);
            _cameraShaker.ShakeCamera();

            var projectile = FindObjectOfType<Projectile>();
            if (projectile)
            {
                FindObjectOfType<CameraShaker>().RestoreValues();
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
                _isDead = true;

                if (session.Tries > 0)
                {
                    SetObjectStatus(false, _leftWingDamage, _rightWingDamage, _bodyDamage, gameObject);
                    _safeZone.GetComponent<Collider2D>().enabled = true;
                    _safeZoneEffect.SetActive(true);
                    _safeZone.GetComponent<TimerComponent>().SetTimer(0);
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

            if (leftTurn)
            {
                _animator.SetTrigger(HitLeftKey);
                if (_isMovingForward) return;
            }
            if (rightTurn)
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

        public void RevivePlayer()
        {
            _isDead = false;
        }
    }
}

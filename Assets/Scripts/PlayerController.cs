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
        [SerializeField] private GameObject _hitParticles;
        [SerializeField] private TimerComponent _timerToContinue;
        [SerializeField] private TimerComponent _timerToGameOver;
        [SerializeField] private GameObject _player;
        [SerializeField] private AudioSource _mainTheme;

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
            var session = FindObjectOfType<GameSession>();

            Instantiate(_hitParticles, other.GetContact(0).point, Quaternion.identity);
            _cameraShaker.ShakeCamera();

            if (_health.Health == 3)
                _leftWingDamage.SetActive(true);
            if (_health.Health == 2)
                _rightWingDamage.SetActive(true);
            if (_health.Health == 1)
            {
                _bodyDamage.SetActive(true);
                _animator.SetBool(DangerHpKey, true);
            }
            if (_health.Health <= 0 && session.Tries > 0)
            {               
                _leftWingDamage.SetActive(false);
                _rightWingDamage.SetActive(false);
                _bodyDamage.SetActive(false);
                _player.SetActive(false);

                _timerToContinue.SetTimer(0);
            }
            if (_health.Health <= 0 && session.Tries == 0)
            {
                _player.SetActive(false);
                _mainTheme.Stop();
                _timerToGameOver.SetTimer(0);
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

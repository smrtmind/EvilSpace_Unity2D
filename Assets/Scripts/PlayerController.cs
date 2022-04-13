using UnityEngine;

namespace Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 150;
        [SerializeField] private float _burstSpeed = 1;
        [SerializeField] private GameObject _leftStarterFlame;
        [SerializeField] private GameObject _rightStarterFlame;
        
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

        private readonly int LeftStarterKey = Animator.StringToHash("left-turn");
        private readonly int RightStarterKey = Animator.StringToHash("right-turn");

        public float burst { get; set; }
        public bool firstWeapon { get; set; }
        public bool secondWeapon { get; set; }

        private Animator _animator;
        private Rigidbody2D _body;
        private AudioSource _audio;

        private void Start()
        {
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
            var isLeftPressed = Input.GetKey(KeyCode.LeftArrow);
            var isRightPressed = Input.GetKey(KeyCode.RightArrow);
            var isMovingForward = burst > 0;

            if (isLeftPressed)
            {
                _body.angularVelocity = 1 * _rotationSpeed;

                PlayAnimation(LeftStarterKey);
                ActivateObject(_rightStarterFlame);
            }
            else if (isRightPressed)
            {
                _body.angularVelocity = -1 * _rotationSpeed;

                PlayAnimation(RightStarterKey);
                ActivateObject(_leftStarterFlame);
            }
            else
            {
                PlayAnimation(disableAll: true);
                ActivateObject(disableAll: true);
            }

            if (isMovingForward)
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
                _leftStarterFlame.SetActive(state);
                _rightStarterFlame.SetActive(state);
            }
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var asteroid = other.gameObject.GetComponent<Asteroid>();

            _audio.PlayOneShot(_shipHit);
        }
    }
}

using UnityEngine;

namespace Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _speed = 5f;
        [SerializeField] private Cooldown _shootingDelay;
        [SerializeField] private Projectile _weapon;
        [SerializeField] private Transform _weaponSpawnPosition;

        private Transform _player;
        private Rigidbody2D _bullet;
        private GameSession _gameSession;
        private float _zAngle;
        private bool _isStopped;

        private void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        private void Start()
        {
            _bullet = GetComponent<Rigidbody2D>();

            GetVectorDirection();
            LookOnPlayerImmediate();
        }

        private void Update()
        {
            if (_isStopped)
            {
                transform.position = transform.position;
            } 
            else
            {
                GetVectorDirection();
            }

            //calculating rotation
            Quaternion desiredRotation = Quaternion.Euler(0, 0, _zAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);

        }

        private void LookOnPlayerImmediate()
        {
            transform.rotation = Quaternion.Euler(0, 0, _zAngle);
        }

        private void GetVectorDirection()
        {
            Vector3 playerPosition = transform.position;
            Vector3 velocity = new Vector3(0, _speed * Time.deltaTime, 0);

            playerPosition += transform.rotation * velocity;
            transform.position = playerPosition;

            var player = FindObjectOfType<PlayerController>();
            if (player != null)
            {
                _player = player.transform;
            }
            else
            {
                return;
            }

            Vector3 direction = _player.position - transform.position;
            _zAngle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg - 90;
        }

        public void AddXp(int xp)
        {
            _gameSession.ModifyXp(xp);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var isPlayer = other.gameObject.tag == "Player";
            if (isPlayer)
            {
                FindObjectOfType<CameraShaker>().RestoreValues();

                var force = transform.position - other.transform.position;
                force.Normalize();

                FindObjectOfType<PlayerController>().GetComponent<Rigidbody2D>().AddForce(-force * 500);
            }
        }

        public void Shoot()
        {
            if (_shootingDelay.IsReady)
            {
                var projectile = Instantiate(_weapon, _weaponSpawnPosition.position, transform.rotation);
                projectile.Launch(_bullet.velocity, transform.up);
                _shootingDelay.Reset();
            }
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            var isPlayer = other.gameObject.tag == "Player";
            if (isPlayer)
                Shoot();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            var isPlayer = other.gameObject.tag == "Player";
            if (isPlayer)
                _isStopped = false;
        }

        public void StopMoving() => _isStopped = true;
    }
}

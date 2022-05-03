using System;
using UnityEngine;

namespace Scripts
{
    public class EnemyAI : MonoBehaviour
    {
        [SerializeField] protected float _rotationSpeed = 100f;
        [SerializeField] protected float _speed = 5f;
        [SerializeField] protected Cooldown _shootingDelay;
        [SerializeField] protected Projectile _weapon;
        [SerializeField] protected Transform _weaponSpawnPosition;
        [SerializeField] private Weapon[] _weapons;

        protected Transform _player;
        protected Rigidbody2D _playerBody;
        protected GameSession _gameSession;
        protected float _zAngle;
        protected bool _isStopped;
        protected CameraShaker _cameraShaker;

        protected virtual void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _cameraShaker = FindObjectOfType<CameraShaker>();
        }

        protected virtual void Start()
        {
            _playerBody = GetComponent<Rigidbody2D>();

            GetVectorDirection();
            LookOnPlayerImmediate();
        }

        protected virtual void Update()
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

        protected virtual void LookOnPlayerImmediate()
        {
            var isBoss = GetComponent<BossAI>();
            if (isBoss) return;

            transform.rotation = Quaternion.Euler(0, 0, _zAngle);
        }

        protected virtual void GetVectorDirection()
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

        protected virtual void AddXp(int xp)
        {
            _gameSession.ModifyXp(xp);
        }

        protected virtual void OnCollisionEnter2D(Collision2D other)
        {
            var isPlayer = other.gameObject.tag == "Player";
            if (isPlayer)
            {
                _cameraShaker.RestoreValues();

                var force = transform.position - other.transform.position;
                force.Normalize();

                FindObjectOfType<PlayerController>().GetComponent<Rigidbody2D>().AddForce(-force * 500);
            }
        }

        protected virtual void Shoot()
        {
            if (_shootingDelay.IsReady)
            {
                var projectile = Instantiate(_weapon, _weaponSpawnPosition.position, transform.rotation);
                projectile.Launch(_playerBody.velocity, transform.up);
                _shootingDelay.Reset();
            }
        }

        protected virtual void OnTriggerStay2D(Collider2D other)
        {
            var isPlayer = other.gameObject.tag == "Player";
            if (isPlayer)
                Shoot();
        }

        protected virtual void OnTriggerExit2D(Collider2D other)
        {
            var isPlayer = other.gameObject.tag == "Player";
            if (isPlayer)
                _isStopped = false;
        }

        protected virtual void StopMoving() => _isStopped = true;
    }

    [Serializable]
    public class Weapon
    {
        [SerializeField] private string _name;
        //[SerializeField] private Sprite[] _charasteristics;

        public string Name => _name;
        //public Sprite[] Sprites => _sprites;
    }
}

using CodeBase.Player;
using CodeBase.Service;
using Scripts;
using System;
using UnityEngine;

namespace CodeBase.Mobs
{
    public class EnemyAI : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private PlayerStorage playerStorage;

        [Header("Movement charasteristics")]
        [SerializeField] private bool _canMove;
        [SerializeField] private float _rotationSpeed = 100f;
        [SerializeField] private float _speed = 5f;

        [Space]
        [SerializeField] private bool _canShoot;
        [SerializeField] private Weapons[] _weapons;

        private Transform _player;
        private Rigidbody2D _playerBody;
        private GameSession _gameSession;
        private float _zAngle;
        private bool _isStopped;
        private CameraShaker _cameraShaker;

        private void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _cameraShaker = FindObjectOfType<CameraShaker>();
        }

        private void Start()
        {
            _playerBody = GetComponent<Rigidbody2D>();

            GetPlayerDirection();
            LookOnPlayerImmediate();
        }

        private void Update()
        {
            if (_canMove && !_isStopped)
            {
                Move();
            }

            GetPlayerDirection();

            //calculating rotation
            Quaternion desiredRotation = Quaternion.Euler(0, 0, _zAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, _rotationSpeed * Time.deltaTime);
        }

        private void LookOnPlayerImmediate()
        {
            transform.rotation = Quaternion.Euler(0, 0, _zAngle);
        }

        private void GetPlayerDirection()
        {
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
            var player = FindObjectOfType<PlayerController>();
            if (player)
            {
                if (!playerStorage.ConcretePlayer.IsDead)
                {
                    _cameraShaker.RestoreValues();

                    var force = transform.position - other.transform.position;
                    force.Normalize();

                    player.GetComponent<Rigidbody2D>().AddForce(-force * 500);
                }
            }
        }

        public void Shoot()
        {
            if (_canShoot)
            {
                for (int i = 0; i < _weapons.Length; i++)
                {
                    if (_weapons[i].ShootingDelay.IsReady)
                    {
                        if (!_weapons[i].SpawnWeaponPoint)
                        {
                            var projectile = Instantiate(_weapons[i].Weapon, _weapons[i].WeaponShootingPoint.position, transform.rotation);
                            projectile.Launch(_playerBody.velocity, transform.up);
                            _weapons[i].ShootingDelay.Reset();
                        }
                        else
                        {
                            _weapons[i].SpawnWeaponPoint.Spawn();
                            _weapons[i].ShootingDelay.Reset();
                        }
                    }
                }
            }
        }

        private void Move()
        {
            Vector3 position = transform.position;
            Vector3 velocity = new Vector3(0, _speed * Time.deltaTime, 0);

            position += transform.rotation * velocity;
            transform.position = position;
        }

        public void StopMoving() => _isStopped = true;

        private void OnTriggerExit2D(Collider2D other)
        {
            var isPlayer = other.gameObject.tag == "Player";
            if (isPlayer)
                _isStopped = false;
        }

        public void OnBossDie() => _gameSession.RestoreEnemies();
    }

    [Serializable]
    public class Weapons
    {
        [SerializeField] private string _name;
        [SerializeField] private Projectile _weapon;
        [SerializeField] private Cooldown _shootingDelay;
        [SerializeField] private Transform _weaponShootingPoint;
        [SerializeField] private SpawnComponent _spawnWeaponPoint;

        public string Name => _name;
        public Projectile Weapon => _weapon;
        public Cooldown ShootingDelay => _shootingDelay;
        public Transform WeaponShootingPoint => _weaponShootingPoint;
        public SpawnComponent SpawnWeaponPoint => _spawnWeaponPoint;
    }
}

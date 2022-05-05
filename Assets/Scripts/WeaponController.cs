using System;
using UnityEngine;

namespace Scripts
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private WeaponSettings[] _weaponSettings;

        public WeaponSettings[] WeaponSettings => _weaponSettings;

        [Space]
        [SerializeField] private Cooldown _reloadingSpeed;
        [SerializeField] private int _bombReloadingDelay;
        [SerializeField] private SpawnComponent _bombEffect;
        [SerializeField] private SpawnComponent _electroEffect;

        [Space]
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _electroShield;
        [SerializeField] private AudioSource _bombReloaded;

        public int BombReloadingDelay => _bombReloadingDelay;
        public GameObject Shield => _shield;

        private PlayerController _playerInput;
        private Rigidbody2D _playerBody;
        private CameraShaker _cameraShaker;

        private int _defaultBombTimer;
        private int _minBombTimer = 30;
        public bool _bombIsReady;

        private Projectile _weapon;
        private Cooldown _shootingDelay;
        private Cooldown _reloadingDelay;
        private Transform _weaponShootingPoint;
        private int _ammoToReload;
        private int _ammoPerShoot;
        private int _currentWeaponType;

        private void Awake()
        {
            foreach (var weapon in _weaponSettings)
            {
                weapon.DefaultAmmo = weapon.Ammo;
            }

            _defaultBombTimer = _bombReloadingDelay;

            _cameraShaker = FindObjectOfType<CameraShaker>();
        }

        private void Start()
        {
            foreach (var weapon in _weaponSettings)
            {
                weapon.Ammo = weapon.DefaultAmmo;
            }

            _bombReloadingDelay = _defaultBombTimer;

            _playerInput = GetComponent<PlayerController>();
            _playerBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_playerInput.firstWeapon)
            {
                if (!_playerInput.secondWeapon)
                {
                    _currentWeaponType = SetWeaponActive(1);
                    Reload();
                }

                _currentWeaponType = SetWeaponActive(0);
                if (_shootingDelay.IsReady && _weaponSettings[0].Ammo > 0)
                {
                    Shoot();
                }
            }

            if (_playerInput.secondWeapon)
            {
                if (!_playerInput.firstWeapon)
                {
                    _currentWeaponType = SetWeaponActive(0);
                    Reload();
                }

                _currentWeaponType = SetWeaponActive(1);
                if (_shootingDelay.IsReady && _weaponSettings[1].Ammo > 0)
                {
                    Shoot();
                }
            }

            if (!_playerInput.firstWeapon && !_playerInput.secondWeapon)
            {
                for (int i = 0; i < _weaponSettings.Length; i++)
                {
                    _currentWeaponType = SetWeaponActive(i);
                    Reload();
                }
            }

            if (_playerInput.thirdWeapon && _bombIsReady)
            {
                UseBomb();
            }

            if (!_bombIsReady)
            {
                ReloadBomb();
            }
        }

        public void PowerUp()
        {
            foreach (var weapon in _weaponSettings)
            {
                weapon.DefaultAmmo += weapon.AmmoToAddOnPowerUp;
                if (weapon.DefaultAmmo >= weapon.MaxAmmo)
                {
                    weapon.DefaultAmmo = weapon.MaxAmmo;
                }
                    
                weapon.ShootingDelay.Value += weapon.ShootingDelayOnPowerUp;
                if (weapon.ShootingDelay.Value <= weapon.ShootingDelayMin)
                {
                    weapon.ShootingDelay.Value = weapon.ShootingDelayMin;
                }                    
            }

            _defaultBombTimer -= 2;

            if (_defaultBombTimer <= _minBombTimer)
                _defaultBombTimer = _minBombTimer;

            FindObjectOfType<GameSession>()._isLevelUp = false;
        }

        public void ActivateElectroShield()
        {
            _electroShield.SetActive(true);
            _electroShield.GetComponent<TimerComponent>().SetTimer(0);
        }

        private int SetWeaponActive(int type)
        {
            _ammoPerShoot = _weaponSettings[type].AmmoPerShoot;
            _ammoToReload = _weaponSettings[type].AmmoToReload;
            _weapon = _weaponSettings[type].Weapon;
            _shootingDelay = _weaponSettings[type].ShootingDelay;
            _reloadingDelay = _weaponSettings[type].ReloadingDelay;
            _weaponShootingPoint = _weaponSettings[type].WeaponShootingPoint;

            return type;
        }

        private void Shoot()
        {
            var projectile = Instantiate(_weapon, _weaponShootingPoint.position, transform.rotation);
            projectile.Launch(_playerBody.velocity, transform.up);

            _weaponSettings[_currentWeaponType].Ammo += _ammoPerShoot;
            _shootingDelay.Reset();
        }

        private void Reload()
        {
            if (_reloadingDelay.IsReady)
            {
                if (_weaponSettings[_currentWeaponType].Ammo != _weaponSettings[_currentWeaponType].DefaultAmmo)
                {
                    _weaponSettings[_currentWeaponType].Ammo += _ammoToReload;
                    _reloadingDelay.Reset();
                }
            }
        }

        public void UseBomb()
        {
            _electroEffect.Spawn();
            _bombEffect.Spawn();

            _cameraShaker.SetDuration(1.2f);
            _cameraShaker.SetMaxDelta(0.6f);
            _cameraShaker.ShakeCamera();

            KillAllEnemies();

            _bombIsReady = false;
            _bombReloadingDelay = _defaultBombTimer;
        }

        private void ReloadBomb()
        {
            if (_reloadingSpeed.IsReady)
            {
                _bombReloadingDelay--;
                _reloadingSpeed.Reset();

                if (_bombReloadingDelay == 0)
                {
                    _bombReloaded.Play();
                    _bombIsReady = true;
                }
            }
        }

        public void KillAllEnemies()
        {
            var asteroids = FindObjectsOfType<Asteroid>();
            foreach (var asteroid in asteroids)
            {
                var asteroidHp = asteroid.GetComponent<HealthComponent>();
                if (asteroidHp)
                    asteroidHp.ModifyHealth(-asteroidHp.Health);
            }

            var ships = FindObjectsOfType<EnemyAI>();
            foreach (var ship in ships)
            {
                var shipHp = ship.GetComponent<HealthComponent>();
                //shipHp.ModifyHealth(-shipHp.Health);
                shipHp.ModifyHealth(-50);
            }

            var projectiles = FindObjectsOfType<Projectile>();
            foreach (var projectile in projectiles)
            {
                Destroy(projectile.gameObject);
            }
        }
    }

    [Serializable]
    public class WeaponSettings
    {
        [SerializeField] private string _name;
        [SerializeField] private Projectile _weapon;
        [SerializeField] private Transform _weaponShootingPoint;

        [Space]
        [SerializeField] private int _ammo;
        [SerializeField] private int _maxAmmo;
        [SerializeField] private Cooldown _reloadingDelay;
        [SerializeField] private int _ammoToReload;
        [SerializeField] private int _ammoPerShoot;
        [SerializeField] private int _ammoToAddOnPowerUp;

        [Space]
        [SerializeField] private Cooldown _shootingDelay;
        [SerializeField] private float _shootingDelayOnPowerUp;
        [SerializeField] private float _shootingDelayMin;

        public string Name => _name;
        public Projectile Weapon => _weapon;
        public Transform WeaponShootingPoint => _weaponShootingPoint;

        public int Ammo
        {
            get => _ammo;
            set => _ammo = value;
        }

        public int MaxAmmo => _maxAmmo;
        public int DefaultAmmo { get; set; }
        public Cooldown ReloadingDelay => _reloadingDelay;
        public int AmmoToReload => _ammoToReload;
        public int AmmoPerShoot => _ammoPerShoot;
        public int AmmoToAddOnPowerUp => _ammoToAddOnPowerUp;
        public Cooldown ShootingDelay => _shootingDelay;
        public float ShootingDelayOnPowerUp => _shootingDelayOnPowerUp;
        public float ShootingDelayMin => _shootingDelayMin;
    }
}

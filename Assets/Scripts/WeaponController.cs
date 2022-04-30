using UnityEngine;

namespace Scripts
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Projectile _gun;
        [SerializeField] private int _gunAmmo;
        [SerializeField] private Cooldown _gunShootingDelay;
        [SerializeField] private Cooldown _gunReloadingDelay;
        [SerializeField] private Transform _gunSpawnPosition;

        [Space]
        [SerializeField] private Projectile _blaster;
        [SerializeField] private int _blasterAmmo;
        [SerializeField] private Cooldown _blasterShootingDelay;
        [SerializeField] private Cooldown _blasterReloadingDelay;
        [SerializeField] private Transform _blasterSpawnPosition;

        [Space]
        [SerializeField] private Cooldown _reloadingSpeed;
        [SerializeField] private int _bombReloadingDelay;
        [SerializeField] private SpawnComponent _bombEffect;
        [SerializeField] private SpawnComponent _electroEffect;

        [Space]
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _electroShield;
        [SerializeField] private AudioSource _bombReloaded;

        public int GunAmmo => _gunAmmo;
        public int BlasterAmmo => _blasterAmmo;
        public int BombReloadingDelay => _bombReloadingDelay;

        private PlayerController _player;
        private Rigidbody2D _bullet;
        private CameraShaker _cameraShaker;

        private int _defaultGunAmmo;
        private const int _maxGunAmmo = 900;
        private float _maxGunFireDensity = 0.05f;

        private int _defaultBlasterAmmo;
        private const int _maxLaserAmmo = 450;
        private float _maxLaserFireDensity = 0.1f;

        private int _defaultBombTimer;
        private int _minBombTimer = 30;
        public bool _bombIsReady;

        private Projectile _currentWeapon;
        private Cooldown _shootingDelay;
        private Cooldown _reloadingDelay;
        private Transform _bulletSpawnPosition;

        private void Awake()
        {
            _defaultGunAmmo = _gunAmmo;
            _defaultBlasterAmmo = _blasterAmmo;
            _defaultBombTimer = _bombReloadingDelay;

            _cameraShaker = FindObjectOfType<CameraShaker>();
        }

        private void Start()
        {
            _gunAmmo = _defaultGunAmmo;
            _blasterAmmo = _defaultBlasterAmmo;
            _bombReloadingDelay = _defaultBombTimer;

            _player = GetComponent<PlayerController>();
            _bullet = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_player.firstWeapon)
            {
                if (!_player.secondWeapon)
                {
                    SetWeaponActive(2);
                    Reload(ref _blasterAmmo, ref _defaultBlasterAmmo, 1);
                }

                if (_gunShootingDelay.IsReady && _gunAmmo > 0)
                {
                    SetWeaponActive(1);
                    Shoot(ref _gunAmmo, 2);
                }
            }

            if (_player.secondWeapon)
            {
                if (!_player.firstWeapon)
                {
                    SetWeaponActive(1);
                    Reload(ref _gunAmmo, ref _defaultGunAmmo, 2);
                }

                if (_blasterShootingDelay.IsReady && _blasterAmmo > 0)
                {
                    SetWeaponActive(2);
                    Shoot(ref _blasterAmmo, 1);
                }
            }

            if (!_player.firstWeapon && !_player.secondWeapon)
            {
                SetWeaponActive(1);
                Reload(ref _gunAmmo, ref _defaultGunAmmo, 2);

                SetWeaponActive(2);
                Reload(ref _blasterAmmo, ref _defaultBlasterAmmo, 1);
            }

            if (_player.thirdWeapon && _bombIsReady)
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
            _shield.SetActive(true);
            _shield.GetComponent<TimerComponent>().SetTimer(0);
            _player._levelUpEffect.Spawn();

            //gun improvements
            if (_defaultGunAmmo != _maxGunAmmo)
                _defaultGunAmmo += 90;

            _gunShootingDelay.Value -= 0.01f;

            if (_gunShootingDelay.Value <= _maxGunFireDensity)
                _gunShootingDelay.Value = _maxGunFireDensity;

            //laser improvements
            if (_defaultBlasterAmmo != _maxLaserAmmo)
                _defaultBlasterAmmo += 45;

            _blasterShootingDelay.Value -= 0.1f;

            if (_blasterShootingDelay.Value <= _maxLaserFireDensity)
                _blasterShootingDelay.Value = _maxLaserFireDensity;

            //bomb improvements
            _defaultBombTimer -= 5;

            if (_defaultBombTimer <= _minBombTimer)
                _defaultBombTimer = _minBombTimer;

            FindObjectOfType<GameSession>()._isLevelUp = false;
        }

        public void ActivateElectroShield()
        {
            _electroShield.SetActive(true);
            _electroShield.GetComponent<TimerComponent>().SetTimer(0);
        }

        private void SetWeaponActive(int weapon)
        {
            switch (weapon)
            {
                case 1:
                    _currentWeapon = _gun;
                    _shootingDelay = _gunShootingDelay;
                    _reloadingDelay = _gunReloadingDelay;
                    _bulletSpawnPosition = _gunSpawnPosition;

                    break;

                case 2:
                    _currentWeapon = _blaster;
                    _shootingDelay = _blasterShootingDelay;
                    _reloadingDelay = _blasterReloadingDelay;
                    _bulletSpawnPosition = _blasterSpawnPosition;

                    break;
            }
        }

        private void Shoot(ref int ammo, int ammoPerShoot)
        {
            var projectile = Instantiate(_currentWeapon, _bulletSpawnPosition.position, transform.rotation);
            projectile.Launch(_bullet.velocity, transform.up);
            ammo -= ammoPerShoot;
            _shootingDelay.Reset();
        }

        private void Reload(ref int ammo, ref int defaultAmmo, int ammoToAdd)
        {
            if (_reloadingDelay.IsReady)
            {
                if (ammo != defaultAmmo)
                {
                    ammo += ammoToAdd;
                    _reloadingDelay.Reset();
                }
            }
        }

        private void UseBomb()
        {
            _electroEffect.Spawn();
            _bombEffect.Spawn();

            _cameraShaker.SetDuration(1.2f);
            _cameraShaker.SetMaxDelta(0.6f);
            _cameraShaker.ShakeCamera();

            var asteroids = FindObjectsOfType<Asteroid>();
            foreach (var asteroid in asteroids)
            {
                var asteroidHp = asteroid.GetComponent<HealthComponent>();
                asteroidHp.ModifyHealth(-asteroidHp.Health);
            }

            var ships = FindObjectsOfType<EnemyAI>();
            foreach (var ship in ships)
            {
                var shipHp = ship.GetComponent<HealthComponent>();
                shipHp.ModifyHealth(-shipHp.Health);
            }

            var projectiles = FindObjectsOfType<Projectile>();
            foreach (var projectile in projectiles)
            {
                Destroy(projectile.gameObject);
            }

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
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Projectile _gun;
        [SerializeField] private int _gunAmmo;
        [SerializeField] private Cooldown _gunShootingDelay;
        [SerializeField] private Cooldown _gunReloadingDelay;
        [SerializeField] private Transform _gunSpawnPosition;
        [SerializeField] private Text _gunHudValue;

        [Space]
        [SerializeField] private Projectile _laser;
        [SerializeField] private int _laserAmmo;
        [SerializeField] private Cooldown _laserShootingDelay;
        [SerializeField] private Cooldown _laserReloadingDelay;
        [SerializeField] private Transform _laserSpawnPosition;
        [SerializeField] private Text _laserHudValue;

        [Space]
        [SerializeField] private Cooldown _reloadingSpeed;
        [SerializeField] private int _bombReloadingDelay;
        [SerializeField] private SpawnComponent _bombEffect;
        [SerializeField] private SpawnComponent _electroEffect;
        [SerializeField] private Text _bombHudStatus;

        [Space]
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _electroShield;
        [SerializeField] private AudioSource _bombReloaded;

        private PlayerController _player;
        private Rigidbody2D _bullet;            

        private int _defaultGunAmmo;
        private const int _maxGunAmmo = 900;
        private float _maxGunFireDensity = 0.05f;

        private int _defaultLaserAmmo;
        private const int _maxLaserAmmo = 450;
        private float _maxLaserFireDensity = 0.1f;

        private int _defaultBombTimer;
        private int _minBombTimer = 30;
        private bool _bombIsReady;

        private Projectile currentWeapon;
        private Cooldown shootingDelay;
        private Cooldown reloadingDelay;
        private Transform bulletSpawnPosition;

        private void Awake()
        {
            _defaultGunAmmo = _gunAmmo;
            _defaultLaserAmmo = _laserAmmo;
            _defaultBombTimer = _bombReloadingDelay;
        }

        private void Start()
        {
            _gunAmmo = _defaultGunAmmo;
            _laserAmmo = _defaultLaserAmmo;
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
                    Reload(ref _laserAmmo, ref _defaultLaserAmmo, 1);
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

                if (_laserShootingDelay.IsReady && _laserAmmo > 0)
                {
                    SetWeaponActive(2);
                    Shoot(ref _laserAmmo, 1);
                }
            }

            if (!_player.firstWeapon && !_player.secondWeapon)
            {
                SetWeaponActive(1);
                Reload(ref _gunAmmo, ref _defaultGunAmmo, 2);

                SetWeaponActive(2);
                Reload(ref _laserAmmo, ref _defaultLaserAmmo, 1);
            }

            //using mega bomb
            if (_player.thirdWeapon && _bombIsReady)
            {
                var cameraShaker = FindObjectOfType<CameraShaker>();

                _electroEffect.Spawn();
                _bombEffect.Spawn();

                cameraShaker.SetDuration(1.2f);
                cameraShaker.SetMaxDelta(0.6f);
                cameraShaker.ShakeCamera();

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
            if (!_bombIsReady)
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

        private void FixedUpdate()
        {
            _gunHudValue.text = $"{_gunAmmo}";
            _laserHudValue.text = $"{_laserAmmo}";

            if (!_bombIsReady)
            {
                _bombHudStatus.color = Color.white;
                _bombHudStatus.text = $"{_bombReloadingDelay}";
            }
            else
            {
                _bombHudStatus.color = Color.green;
                _bombHudStatus.text = $"OK";
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
            if (_defaultLaserAmmo != _maxLaserAmmo)
                _defaultLaserAmmo += 45;

            _laserShootingDelay.Value -= 0.1f;

            if (_laserShootingDelay.Value <= _maxLaserFireDensity)
                _laserShootingDelay.Value = _maxLaserFireDensity;

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
                    currentWeapon = _gun;
                    shootingDelay = _gunShootingDelay;
                    reloadingDelay = _gunReloadingDelay;
                    bulletSpawnPosition = _gunSpawnPosition;

                    break;

                case 2:
                    currentWeapon = _laser;
                    shootingDelay = _laserShootingDelay;
                    reloadingDelay = _laserReloadingDelay;
                    bulletSpawnPosition = _laserSpawnPosition;

                    break;
            }
        }

        private void Shoot(ref int ammo, int ammoPerShoot)
        {
            var projectile = Instantiate(currentWeapon, bulletSpawnPosition.position, transform.rotation);
            projectile.Launch(_bullet.velocity, transform.up);
            ammo -= ammoPerShoot;
            shootingDelay.Reset();
        }

        private void Reload(ref int ammo, ref int defaultAmmo, int ammoToAdd)
        {
            if (reloadingDelay.IsReady)
            {
                if (ammo != defaultAmmo)
                {
                    ammo += ammoToAdd;
                    reloadingDelay.Reset();
                }
            }
        }
    }
}

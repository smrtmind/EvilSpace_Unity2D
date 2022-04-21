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

        private PlayerController _player;
        private Rigidbody2D _bullet;
        private int _maxGunAmmo;
        private int _maxLaserAmmo;
        private int _defaultBombTimer;
        private bool _bombIsReady;

        private void Awake()
        {
            _defaultBombTimer = _bombReloadingDelay;
            _maxGunAmmo = _gunAmmo;
            _maxLaserAmmo = _laserAmmo;
        }

        private void Start()
        {
            _player = GetComponent<PlayerController>();
            _bullet = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_player.firstWeapon && _gunShootingDelay.IsReady && !_player.secondWeapon && _gunAmmo > 0)
            {
                if (_laserReloadingDelay.IsReady)
                {
                    if (_laserAmmo != _maxLaserAmmo)
                    {
                        _laserAmmo += 2;
                        _laserReloadingDelay.Reset();
                    }
                }

                var projectile = Instantiate(_gun, _gunSpawnPosition.position, transform.rotation);
                projectile.Launch(_bullet.velocity, transform.up);
                _gunAmmo--;
                _gunShootingDelay.Reset();
                
            }

            if (_player.secondWeapon && _laserShootingDelay.IsReady && !_player.firstWeapon && _laserAmmo > 0)
            {
                if (_gunReloadingDelay.IsReady)
                {
                    if (_gunAmmo != _maxGunAmmo)
                    {
                        _gunAmmo++;
                        _gunReloadingDelay.Reset();
                    }
                }

                var projectile = Instantiate(_laser, _laserSpawnPosition.position, transform.rotation);
                projectile.Launch(_bullet.velocity, transform.up);
                _laserAmmo -= 2;
                _laserShootingDelay.Reset();              
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
                    asteroid.GetComponent<HealthComponent>().ModifyHealth(-100);
                }

                var ships = FindObjectsOfType<EnemyAI>();
                foreach (var ship in ships)
                {
                    ship.GetComponent<HealthComponent>().ModifyHealth(-100);
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
                        _bombIsReady = true;
                    }  
                }
            }

            if (!_player.firstWeapon && !_player.secondWeapon)
            {
                if (_laserReloadingDelay.IsReady)
                {
                    if (_laserAmmo != _maxLaserAmmo)
                    {
                        _laserAmmo += 2;
                        _laserReloadingDelay.Reset();
                    }
                }

                if (_gunReloadingDelay.IsReady)
                {
                    if (_gunAmmo != _maxGunAmmo)
                    {
                        _gunAmmo++;
                        _gunReloadingDelay.Reset();
                    }
                }
            }
        }

        private void FixedUpdate()
        {
            _gunHudValue.text = $"{_gunAmmo} x";
            _laserHudValue.text = $"{_laserAmmo} x";

            if (!_bombIsReady)
            {
                _bombHudStatus.color = Color.white;
                _bombHudStatus.text = $"{_bombReloadingDelay}";
            }
            else
            {
                _bombHudStatus.color = Color.green;
                _bombHudStatus.text = $"ready";
            }
        }
    }
}

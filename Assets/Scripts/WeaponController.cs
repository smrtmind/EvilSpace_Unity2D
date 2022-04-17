using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Projectile _gun;
        [SerializeField] private int _gunAmmo;
        [SerializeField] private Cooldown _gunShootingtDelay;
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

        private PlayerController _player;
        private Rigidbody2D _body;
        private int _maxGunAmmo;
        private int _maxLaserAmmo;

        private void Awake()
        {
            _maxGunAmmo = _gunAmmo;
            _maxLaserAmmo = _laserAmmo;
        }

        private void Start()
        {
            _player = GetComponent<PlayerController>();
            _body = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_player.firstWeapon && _gunShootingtDelay.IsReady && !_player.secondWeapon && _gunAmmo > 0)
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
                projectile.Launch(_body.velocity, transform.up);
                _gunAmmo--;
                _gunShootingtDelay.Reset();
                
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
                projectile.Launch(_body.velocity, transform.up);
                _laserAmmo -= 2;
                _laserShootingDelay.Reset();              
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
        }
    }
}

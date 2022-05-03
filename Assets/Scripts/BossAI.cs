using UnityEngine;

namespace Scripts
{
    public class BossAI : EnemyAI
    {   
        [Header("Second weapon")]
        [SerializeField] private Cooldown _secondWeaponShootingDelay;
        [SerializeField] private Projectile _secondWeapon;
        [SerializeField] private Transform _secondWeaponSpawnPosition;

        [Header("Third weapon")]
        [SerializeField] private Cooldown _thirdWeaponShootingDelay;
        [SerializeField] private Projectile _thirdWeapon;
        [SerializeField] private Transform _thirdWeaponSpawnPosition;

        [Header("Spawn weapon")]
        [SerializeField] private Cooldown _spawnWeaponShootingDelay;
        [SerializeField] private SpawnComponent _spawnWeaponPosition;

        protected override void Shoot()
        {
            base.Shoot();

            if (_secondWeaponShootingDelay.IsReady)
            {
                var projectile = Instantiate(_secondWeapon, _secondWeaponSpawnPosition.position, transform.rotation);
                projectile.Launch(_playerBody.velocity, transform.up);
                _secondWeaponShootingDelay.Reset();
            }

            if (_thirdWeaponShootingDelay.IsReady)
            {
                var projectile = Instantiate(_thirdWeapon, _thirdWeaponSpawnPosition.position, transform.rotation);
                projectile.Launch(_playerBody.velocity, transform.up);
                _thirdWeaponShootingDelay.Reset();
            }

            if (_spawnWeaponShootingDelay.IsReady)
            {
                _spawnWeaponPosition.Spawn();
                _spawnWeaponShootingDelay.Reset();
            }
        }
    }
}

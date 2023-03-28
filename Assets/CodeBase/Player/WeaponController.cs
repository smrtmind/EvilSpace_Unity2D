using CodeBase.ObjectBased;
using CodeBase.Service;
using CodeBase.Utils;
using Scripts;
using System;
using System.Collections;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Player
{
    public class WeaponController : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;
        //[SerializeField] private WeaponStorage weaponStorage;




        //[SerializeField] private WeaponType currentWeapon;
        [SerializeField] private Projectile blaster;
        [SerializeField] private float blasterDamage;


        [SerializeField] private WeaponSettings[] _weaponSettings;

        public WeaponSettings[] WeaponSettings => _weaponSettings;

        [Space]
        [Header("Mega bomb")]
        [SerializeField] private int _bombTimer;
        [SerializeField] private int _bombTimerMin;
        [SerializeField] private int _decreaseBombTimerOnPowerUp;
        //[SerializeField] private Cooldown _reloadingSpeed;

        [Space]
        [Header("Effects")]
        //[SerializeField] private SpawnComponent _bombEffect;
        //[SerializeField] private SpawnComponent _electroEffect;
        [SerializeField] private GameObject _shield;
        [SerializeField] private GameObject _electroShield;

        private int _bombTimerDefault;
        private bool _bombIsReady;

        public bool BombIsReady => _bombIsReady;
        public int BombTimer => _bombTimer;
        public GameObject Shield => _shield;

        //private PlayerController _playerInput;
        private Rigidbody2D _playerBody;
        private CameraShaker _cameraShaker;
        private AudioComponent _audio;

        private Projectile _weapon;
        //private Cooldown _shootingDelay;
        //private Cooldown _reloadingDelay;
        private Transform _weaponShootingPoint;
        private int _ammoToReload;
        private int _ammoPerShoot;
        private int _currentWeaponType;
        private bool _allWeaponMaxOut;
        private Coroutine shootingCoroutine;

        public bool AllWeaponMaxOut => _allWeaponMaxOut;

        private void Awake()
        {
            foreach (var weapon in _weaponSettings)
            {
                weapon.DefaultAmmo = weapon.Ammo;
            }

            _bombTimerDefault = _bombTimer;

            _cameraShaker = FindObjectOfType<CameraShaker>();
            _audio = FindObjectOfType<AudioComponent>();
        }

        private void OnEnable()
        {
            //SetCurrentWeapon(WeaponType.Blaster);

            TouchController.OnStartMoving += StartShooting;
        }

        private void OnDisable()
        {
            TouchController.OnStartMoving -= StartShooting;
        }

        private void Start()
        {
            foreach (var weapon in _weaponSettings)
            {
                weapon.Ammo = weapon.DefaultAmmo;
            }

            _bombTimer = _bombTimerDefault;

            //_playerInput = GetComponent<PlayerController>();
            _playerBody = GetComponent<Rigidbody2D>();
        }

        private void StartShooting(bool isMoving)
        {
            if (isMoving)
            {
                _currentWeaponType = SetWeaponActive(0);
                shootingCoroutine = StartCoroutine(EndlessShooting());
            }
            else
            {
                StopCoroutine(shootingCoroutine);
            }
        }

        private IEnumerator EndlessShooting()
        {     
            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(0.1f);
            }
        }


        //private void Update()
        //{
        //    if (!_playerInput.secondWeapon)
        //    {
        //        _currentWeaponType = SetWeaponActive(1);
        //        Reload();
        //    }

        //    _currentWeaponType = SetWeaponActive(0);
        //    if (_shootingDelay.IsReady && _weaponSettings[0].Ammo > 0)
        //    {
        //        Shoot();
        //    }
        //}

        //private void Update()
        //{
        //    if (_playerInput.firstWeapon)
        //    {
        //        if (!_playerInput.secondWeapon)
        //        {
        //            _currentWeaponType = SetWeaponActive(1);
        //            Reload();
        //        }

        //        _currentWeaponType = SetWeaponActive(0);
        //        if (_shootingDelay.IsReady && _weaponSettings[0].Ammo > 0)
        //        {
        //            Shoot();
        //        }
        //    }

        //    if (_playerInput.secondWeapon)
        //    {
        //        if (!_playerInput.firstWeapon)
        //        {
        //            _currentWeaponType = SetWeaponActive(0);
        //            Reload();
        //        }

        //        _currentWeaponType = SetWeaponActive(1);
        //        if (_shootingDelay.IsReady && _weaponSettings[1].Ammo > 0)
        //        {
        //            Shoot();
        //        }
        //    }

        //    if (!_playerInput.firstWeapon && !_playerInput.secondWeapon)
        //    {
        //        for (int i = 0; i < _weaponSettings.Length; i++)
        //        {
        //            _currentWeaponType = SetWeaponActive(i);
        //            Reload();
        //        }
        //    }

        //    if (_playerInput.thirdWeapon && _bombIsReady)
        //    {
        //        UseBomb();
        //    }

        //    if (!_bombIsReady)
        //    {
        //        ReloadBomb();
        //    }
        //}

        //public void PowerUp()
        //{
        //    int _maxOutWeaponCounter = default;

        //    foreach (var weapon in _weaponSettings)
        //    {
        //        weapon.DefaultAmmo += weapon.AmmoToAddOnPowerUp;
        //        if (weapon.DefaultAmmo >= weapon.MaxAmmo)
        //        {
        //            weapon.DefaultAmmo = weapon.MaxAmmo;

        //            _maxOutWeaponCounter++;
        //        }
                    
        //        weapon.ShootingDelay.Value += weapon.ShootingDelayOnPowerUp;
        //        if (weapon.ShootingDelay.Value <= weapon.ShootingDelayMin)
        //        {
        //            weapon.ShootingDelay.Value = weapon.ShootingDelayMin;

        //            _maxOutWeaponCounter++;
        //        }                    
        //    }

        //    _bombTimerDefault += _decreaseBombTimerOnPowerUp;
        //    if (_bombTimerDefault <= _bombTimerMin)
        //    {
        //        _bombTimerDefault = _bombTimerMin;

        //        _maxOutWeaponCounter++;
        //    }

        //    //length of array with weapon * 2 (amount of characteristics needed to be max out on each weapon) + 1 (bomb timer, which is not included to weapon array)
        //    if (_maxOutWeaponCounter == _weaponSettings.Length * 2 + 1)
        //    {
        //        _allWeaponMaxOut = true;
        //    }
        //}

        public void ActivateElectroShield()
        {
            _electroShield.SetActive(true);
            //_electroShield.GetComponent<TimerComponent>().SetTimer(0);
        }

        private int SetWeaponActive(int type)
        {
            _ammoPerShoot = _weaponSettings[type].AmmoPerShoot;
            _ammoToReload = _weaponSettings[type].AmmoToReload;
            _weapon = _weaponSettings[type].Weapon;
            //_shootingDelay = _weaponSettings[type].ShootingDelay;
            //_reloadingDelay = _weaponSettings[type].ReloadingDelay;
            _weaponShootingPoint = _weaponSettings[type].WeaponShootingPoint;

            return type;
        }

        private void Shoot()
        {
            var projectile = GetFreeProjectile();
            projectile.SetBusyState(true);
            projectile.transform.position = _weaponShootingPoint.position;
            projectile.transform.rotation = transform.rotation;
                //Instantiate(_weapon, _weaponShootingPoint.position, transform.rotation);
            projectile.Launch(_playerBody.velocity, transform.up);

            //_weaponSettings[_currentWeaponType].Ammo += _ammoPerShoot;
            //_shootingDelay.Reset();
        }

        //private void Reload()
        //{
        //    if (_reloadingDelay.IsReady)
        //    {
        //        if (_weaponSettings[_currentWeaponType].Ammo != _weaponSettings[_currentWeaponType].DefaultAmmo)
        //        {
        //            _weaponSettings[_currentWeaponType].Ammo += _ammoToReload;
        //            //_reloadingDelay.Reset();
        //        }
        //    }
        //}

        public void UseBomb()
        {
            //_electroEffect.Spawn();
            //_bombEffect.Spawn();

            _cameraShaker.SetDuration(1.2f);
            _cameraShaker.SetMaxDelta(0.6f);
            //_cameraShaker.ShakeCamera();

            KillAllEnemies();

            _bombIsReady = false;
            _bombTimer = _bombTimerDefault;
        }

        //private void ReloadBomb()
        //{
        //    if (_reloadingSpeed.IsReady)
        //    {
        //        _bombTimer--;
        //        _reloadingSpeed.Reset();

        //        if (_bombTimer == 0)
        //        {
        //            _audio.Play("bomb reloaded", 0.8f);

        //            _bombIsReady = true;
        //        }
        //    }
        //}

        //private void SetCurrentWeapon(WeaponType type) => currentWeapon = type;

        public void KillAllEnemies()
        {
            //var asteroids = FindObjectsOfType<Asteroid>();
            //foreach (var asteroid in asteroids)
            //{
            //    var asteroidHp = asteroid.GetComponent<HealthComponent>();
            //    if (asteroidHp)
            //        asteroidHp.ModifyHealth(-asteroidHp.Health);
            //}

            //var ships = FindObjectsOfType<ShipAi>();
            //foreach (var ship in ships)
            //{
            //    var shipHp = ship.GetComponent<HealthComponent>();
            //    //shipHp.ModifyHealth(-shipHp.Health);
            //    shipHp.ModifyHealth(-50);
            //}

            var projectiles = FindObjectsOfType<Projectile>();
            foreach (var projectile in projectiles)
            {
                //if (projectile.IsHostile)
                    Destroy(projectile.gameObject);
            }
        }

        public Projectile GetFreeProjectile()
        {
            Projectile freeProjectile = dependencyContainer.ParticlePool.ProjectilesPool.Find(projectile => !projectile.IsBusy && projectile.WeaponType == blaster.WeaponType);
            if (freeProjectile == null)
                freeProjectile = CreateNewProjectile();

            return freeProjectile;
        }

        private Projectile CreateNewProjectile()
        {
            Projectile newProjectile = Instantiate(blaster, dependencyContainer.ParticlePool.ProjectileContainer);
            dependencyContainer.ParticlePool.ProjectilesPool.Add(newProjectile);
            Dictionaries.Projectiles.Add(newProjectile.transform, newProjectile);
            newProjectile.SetDamage(blasterDamage);

            return newProjectile;
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
        //[SerializeField] private Cooldown _reloadingDelay;
        [SerializeField] private int _ammoToReload;
        [SerializeField] private int _ammoPerShoot;
        [SerializeField] private int _ammoToAddOnPowerUp;

        [Space]
        //[SerializeField] private Cooldown _shootingDelay;
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
        //public Cooldown ReloadingDelay => _reloadingDelay;
        public int AmmoToReload => _ammoToReload;
        public int AmmoPerShoot => _ammoPerShoot;
        public int AmmoToAddOnPowerUp => _ammoToAddOnPowerUp;
        //public Cooldown ShootingDelay => _shootingDelay;
        public float ShootingDelayOnPowerUp => _shootingDelayOnPowerUp;
        public float ShootingDelayMin => _shootingDelayMin;
    }
}

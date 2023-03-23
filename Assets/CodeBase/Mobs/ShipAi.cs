using CodeBase.ObjectBased;
using CodeBase.Player;
using CodeBase.Utils;
using System.Collections;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Mobs
{
    public class ShipAi : Enemy
    {
        [SerializeField] private WeaponStorage weaponStorage;

        [Header("Ship Settings")]
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float stopDistance = 10f;
        [SerializeField] private Rigidbody2D shipBody;
        [SerializeField] private WeaponType weaponType;
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private float delayBetweenShoots;

        private float zAngle;
        private PlayerController player;
        private Coroutine moveCoroutine;
        private Coroutine shootingCoroutine;

        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
        }

        private void OnEnable()
        {
            GetPlayerDirection();
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);

            moveCoroutine = StartCoroutine(StartMove());
        }

        private void OnDisable()
        {
            moveCoroutine = null;
            shootingCoroutine = null;

            //GetPlayerDirection();
            //transform.rotation = Quaternion.Euler(0, 0, zAngle);
        }

        //private void LookOnPlayerImmediate() => transform.rotation = Quaternion.Euler(0, 0, _zAngle);

        private void GetPlayerDirection()
        {
            Vector3 direction = player.transform.position - transform.position;
            zAngle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg - 90;

            //calculate rotation
            Quaternion desiredRotation = Quaternion.Euler(0, 0, zAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        //public void AddXp(int xp)
        //{
        //    _gameSession.ModifyXp(xp);
        //}

        //private void OnCollisionEnter2D(Collision2D other)
        //{
        //    var player = FindObjectOfType<PlayerController>();
        //    if (player)
        //    {
        //        if (!playerStorage.ConcretePlayer.IsDead)
        //        {
        //            _cameraShaker.RestoreValues();

        //            var force = transform.position - other.transform.position;
        //            force.Normalize();

        //            player.GetComponent<Rigidbody2D>().AddForce(-force * 500);
        //        }
        //    }
        //}

        //private void CalculateRotation()
        //{
        //    Quaternion desiredRotation = Quaternion.Euler(0, 0, zAngle);
        //    transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        //}

        private IEnumerator StartMove()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                GetPlayerDirection();

                if (Vector3.Distance(transform.position, player.transform.position) > stopDistance)
                {
                    Move();
                }
                else
                {
                    if (shootingCoroutine == null)
                        shootingCoroutine = StartCoroutine(EndlessShooting());
                }
            }
        }

        private void Move()
        {
            Vector3 position = transform.position;
            Vector3 velocity = new Vector3(0, movementSpeed * Time.deltaTime, 0);

            position += transform.rotation * velocity;
            transform.position = position;
        }

        private IEnumerator EndlessShooting()
        {
            while (true)
            {
                var projectile = GetFreeProjectile();
                projectile.SetBusyState(true);
                projectile.transform.position = shootingPoint.position;
                projectile.transform.rotation = transform.rotation;
                projectile.Launch(shipBody.velocity, transform.up);

                yield return new WaitForSeconds(delayBetweenShoots);
            }
        }
        private Projectile GetFreeProjectile()
        {
            Projectile freeProjectile = dependencyContainer.ParticlePool.ProjectilesPool.Find(projectile => !projectile.IsBusy && projectile.WeaponType == weaponType);
            if (freeProjectile == null)
                freeProjectile = CreateNewProjectile();

            return freeProjectile;
        }

        private Projectile CreateNewProjectile()
        {
            Projectile newProjectile = Instantiate(weaponStorage.GetEnemyWeapon(weaponType).Projectile, dependencyContainer.ParticlePool.ProjectileContainer);
            dependencyContainer.ParticlePool.ProjectilesPool.Add(newProjectile);
            Dictionaries.Projectiles.Add(newProjectile.transform, newProjectile);

            return newProjectile;
        }
    }
}

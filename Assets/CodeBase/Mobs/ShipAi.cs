using CodeBase.ObjectBased;
using CodeBase.Player;
using CodeBase.Utils;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CodeBase.Mobs
{
    public class ShipAi : Enemy
    {
        [Header("Ship Settings")]
        [SerializeField] private Projectile weapon;
        [SerializeField] private float shipDamage;
        [SerializeField] private float rotationSpeed = 100f;
        [SerializeField] private float movementSpeed = 5f;
        [SerializeField] private float stopDistance = 10f;
        [SerializeField] private Rigidbody2D shipBody;
        [SerializeField] private Transform shootingPoint;
        [SerializeField] private float delayBetweenShoots;

        [Inject] private DiContainer diContainer;

        private float zAngle;
        private PlayerController playerController;
        private Coroutine moveCoroutine;
        private Coroutine shootingCoroutine;

        [Inject]
        private void Construct(PlayerController player)
        {
            playerController = player;
        }

        private void OnEnable()
        {
            GetPlayerDirection();
            transform.rotation = Quaternion.Euler(0f, 0f, 180f);

            moveCoroutine = StartCoroutine(StartMove());

            PlayerController.OnPlayerDied += ChangeBehaviour;
        }

        private void OnDisable()
        {
            moveCoroutine = null;
            shootingCoroutine = null;

            PlayerController.OnPlayerDied -= ChangeBehaviour;

            //GetPlayerDirection();
            //transform.rotation = Quaternion.Euler(0, 0, zAngle);
        }

        //private void LookOnPlayerImmediate() => transform.rotation = Quaternion.Euler(0, 0, _zAngle);

        private void GetPlayerDirection()
        {
            Vector3 direction = playerController.transform.position - transform.position;
            zAngle = Mathf.Atan2(direction.normalized.y, direction.normalized.x) * Mathf.Rad2Deg - 90;

            //calculate rotation
            Quaternion desiredRotation = Quaternion.Euler(0, 0, zAngle);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
        }

        private IEnumerator StartMove()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                GetPlayerDirection();

                if (Vector3.Distance(transform.position, playerController.transform.position) > stopDistance)
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
            Projectile freeProjectile = particlePool.ProjectilesPool.Find(projectile => !projectile.IsBusy && projectile.WeaponType == weapon.WeaponType);
            if (freeProjectile == null)
                freeProjectile = CreateNewProjectile();

            return freeProjectile;
        }

        private Projectile CreateNewProjectile()
        {
            Projectile newProjectile = diContainer.InstantiatePrefabForComponent<Projectile>(weapon, particlePool.ProjectileContainer);
            particlePool.ProjectilesPool.Add(newProjectile);
            Dictionaries.Projectiles.Add(newProjectile.transform, newProjectile);
            newProjectile.SetDamage(shipDamage);

            return newProjectile;
        }

        private void ChangeBehaviour()
        {
            ////StopCoroutine(moveCoroutine);
            //StopCoroutine(shootingCoroutine); 
            //shootingCoroutine = null;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Player))
            {
                PlayerController.OnPlayerCollision?.Invoke(transform.position);
            }
        }
    }
}

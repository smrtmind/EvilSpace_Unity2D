using CodeBase.Player;
using CodeBase.Utils;
using System.Collections;
using UnityEngine;

namespace CodeBase.ObjectBased
{
    public class Rocket : EnemyProjectile
    {
        [SerializeField] private float rotationSpeed;

        private float zAngle;
        private PlayerController player;
        private Coroutine moveCoroutine;

        private void Awake()
        {
            player = FindObjectOfType<PlayerController>();
        }

        private void OnEnable()
        {
            GetPlayerDirection();
            //transform.rotation = Quaternion.Euler(0f, 0f, 180f);

            moveCoroutine = StartCoroutine(StartMove());
        }

        private void OnDisable()
        {
            moveCoroutine = null;

            //GetPlayerDirection();
            //transform.rotation = Quaternion.Euler(0, 0, zAngle);
        }

        private void GetPlayerDirection()
        {
            Vector3 direction = player.transform.position - transform.position;
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
                Move();

                //if (Vector3.Distance(transform.position, player.transform.position) > stopDistance)
                //{
                //    Move();
                //}
                //else
                //{
                //    if (shootingCoroutine == null)
                //        shootingCoroutine = StartCoroutine(EndlessShooting());
                //}
            }
        }

        private void Move()
        {
            Vector3 position = transform.position;
            Vector3 velocity = new Vector3(0, speed * Time.deltaTime, 0);

            position += transform.rotation * velocity;
            transform.position = position;
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

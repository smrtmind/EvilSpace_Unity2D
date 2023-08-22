using CodeBase.Player;
using CodeBase.Utils;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Mobs
{
    public class Asteroid : Enemy
    {
        [Header("Asteroid Settings")]
        [SerializeField] private Rigidbody2D asteroidBody;

        [Space]
        [SerializeField, Range(1f, 10f)] private float minSpeed = 1f;
        [SerializeField, Range(1f, 10f)] private float maxSpeed = 5f;
        [SerializeField, Range(1f, 50f)] private float minRotation = 5f;
        [SerializeField, Range(1f, 50f)] private float maxRotation = 25;
        [SerializeField] private float minScale;
        [SerializeField] private float maxScale;

        private Vector2 screenBoundaries;
        private Coroutine boundsCoroutine;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            float randomScale = Random.Range(minScale, maxScale);
            transform.localScale = new Vector3(randomScale, randomScale, 1f);

            boundsCoroutine = StartCoroutine(CheckForScreenBounds());
            Launch();
        }

        private void OnDisable()
        {
            StopCoroutine(boundsCoroutine);
        }

        private IEnumerator CheckForScreenBounds()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                screenBoundaries = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

                if (transform.position.y > screenBoundaries.y || transform.position.y < -screenBoundaries.y
                || transform.position.x > screenBoundaries.x || transform.position.x < -screenBoundaries.x)
                {
                    SetBusyState(false);
                    break;
                }
            }
        }

        private void Launch()
        {
            //var randomDirection = Random.insideUnitCircle.normalized;
            asteroidBody.velocity = Vector2.down * Random.Range(minSpeed, maxSpeed);

            var randomRotation = Random.Range(minRotation, maxRotation);
            asteroidBody.AddTorque(randomRotation);
        }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            base.OnTriggerEnter2D(collision);

            if (collision.gameObject.tag.Equals(Tags.Player))
            {
                ModifyHealth(-Health);
                EventObserver.OnPlayerCollision?.Invoke(transform.position);
            }
        }
    }
}

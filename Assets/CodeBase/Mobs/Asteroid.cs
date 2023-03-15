using CodeBase.Service;
using System.Collections;
using UnityEngine;

namespace CodeBase.Mobs
{
    public class Asteroid : Enemy
    {
        [Header("Unit Unique Settings")]
        [SerializeField] private Rigidbody2D asteroidBody;

        [Space]
        [SerializeField] private float _minSpeed = 1f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _minRotation = 5f;
        [SerializeField] private float _maxRotation = 25;
        [SerializeField] private float minScale;
        [SerializeField] private float maxScale;

        private GameSession gameSession;
        private CameraShaker cameraShaker;
        private Vector2 screenBoundaries;

        private void Awake()
        {
            gameSession = FindObjectOfType<GameSession>();
            cameraShaker = FindObjectOfType<CameraShaker>();
        }

        private void OnEnable()
        {
            float randomScale = Random.Range(minScale, maxScale);
            transform.localScale = new Vector3(randomScale, randomScale, 1f);

            Launch();
            StartCoroutine(CheckForScreenBounds());
        }

        private IEnumerator CheckForScreenBounds()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

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
            asteroidBody.velocity = Vector2.down * Random.Range(_minSpeed, _maxSpeed);

            var randomRotation = Random.Range(_minRotation, _maxRotation);
            asteroidBody.AddTorque(randomRotation);
        }

        public void AddXp(int xp)
        {
            gameSession.ModifyXp(xp);
        }

        //private void OnCollisionEnter2D(Collision2D other)
        //{
        //    var player = other.gameObject.CompareTag("Player");
        //    if (player)
        //    {
        //        if (!playerStorage.ConcretePlayer.IsDead)
        //        {
        //            playerStorage.ConcretePlayer.ModifyHealth(-Damage);

        //            _cameraShaker.RestoreValues();

        //            //var force = transform.position - other.transform.position;
        //            //force.Normalize();

        //            //player.GetComponent<Rigidbody2D>().AddForce(-force * 500);
        //        }
        //    }
        //}
    }
}

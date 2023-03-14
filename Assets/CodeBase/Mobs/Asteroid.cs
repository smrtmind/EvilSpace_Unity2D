using CodeBase.Service;
using CodeBase.Utils;
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

        private GameSession _gameSession;
        private CameraShaker _cameraShaker;

        private void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _cameraShaker = FindObjectOfType<CameraShaker>();
        }

        //public override void SetBusyState(bool state)
        //{
        //    IsBusy = state;
        //    gameObject.SetActive(IsBusy);
        //}

        void Update()
        {
            // Get the screen boundaries in world coordinates
            Vector2 screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            // Check if the object is out of the screen boundaries
            if (transform.position.y > screenBoundaries.y || transform.position.y < -screenBoundaries.y
            || transform.position.x > screenBoundaries.x || transform.position.x < -screenBoundaries.x)
            {
                SetBusyState(false);
            }
        }

        public override void Launch()
        {
            //var randomDirection = Random.insideUnitCircle.normalized;
            asteroidBody.velocity = Vector2.down * Random.Range(_minSpeed, _maxSpeed);

            var randomRotation = Random.Range(_minRotation, _maxRotation);
            asteroidBody.AddTorque(randomRotation);
        }

        public void AddXp(int xp)
        {
            _gameSession.ModifyXp(xp);
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

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Projectile))
            {
                SetBusyState(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Projectile))
            {
                SetBusyState(false);
            }
        }
    }
}

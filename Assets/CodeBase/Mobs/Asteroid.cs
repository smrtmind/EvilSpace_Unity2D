using CodeBase.Service;
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

        public void Launch()
        {
            var randomDirection = Random.insideUnitCircle.normalized;
            asteroidBody.velocity = randomDirection * Random.Range(_minSpeed, _maxSpeed);

            var randomRotation = Random.Range(_minRotation, _maxRotation);
            asteroidBody.AddTorque(randomRotation);
        }

        public void AddXp(int xp)
        {
            _gameSession.ModifyXp(xp);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var player = other.gameObject.CompareTag("Player");
            if (player)
            {
                if (!playerStorage.ConcretePlayer.IsDead)
                {
                    playerStorage.ConcretePlayer.ModifyHealth(-damage);

                    _cameraShaker.RestoreValues();

                    //var force = transform.position - other.transform.position;
                    //force.Normalize();

                    //player.GetComponent<Rigidbody2D>().AddForce(-force * 500);
                }
            }
        }
    }
}

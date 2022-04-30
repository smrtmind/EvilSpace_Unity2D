using UnityEngine;

namespace Scripts
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float _minSpeed = 1f;
        [SerializeField] private float _maxSpeed = 5f;
        [SerializeField] private float _minRotation = 5f;
        [SerializeField] private float _maxRotation = 25;

        private Rigidbody2D _body;
        private GameSession _gameSession;
        private CameraShaker _cameraShaker;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _gameSession = FindObjectOfType<GameSession>();
            _cameraShaker = FindObjectOfType<CameraShaker>();
        }

        public void Launch()
        {
            var randomDirection = Random.insideUnitCircle.normalized;
            _body.velocity = randomDirection * Random.Range(_minSpeed, _maxSpeed);

            var randomRotation = Random.Range(_minRotation, _maxRotation);
            _body.AddTorque(randomRotation);
        }

        public void AddXp(int xp)
        {
            _gameSession.ModifyXp(xp);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            var isPlayer = other.gameObject.tag == "Player";
            if (isPlayer)
            {
                _cameraShaker.RestoreValues();

                var force = transform.position - other.transform.position;
                force.Normalize();

                FindObjectOfType<PlayerController>().GetComponent<Rigidbody2D>().AddForce(-force * 500);
            }
        }
    }
}

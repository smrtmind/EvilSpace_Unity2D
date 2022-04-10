using UnityEngine;

namespace Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 150;
        [SerializeField] private float _burstSpeed = 1;

        public float rotation { get; set; }
        public float burst { get; set; }
        public bool shoot { get; set; }

        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>();   
        }

        private void FixedUpdate()
        {
            if (rotation != 0)
            {
                _rigidbody.angularVelocity = rotation * -_rotationSpeed;
            }

            if (burst > 0)
            {
                Vector2 burstDelta = transform.up * _burstSpeed;
                _rigidbody.velocity += burstDelta;
            }
        }
    }
}

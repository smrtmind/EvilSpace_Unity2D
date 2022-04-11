using UnityEngine;

namespace Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 150;
        [SerializeField] private float _burstSpeed = 1;

        private Animator _animator;

        private readonly int LeftTurnKey = Animator.StringToHash("left-turn");
        private readonly int RightTurnKey = Animator.StringToHash("right-turn");

        //public float rotation { get; set; }
        public float burst { get; set; }
        public bool shoot { get; set; }

        private Rigidbody2D _rigidbody;

        private void Start()
        {
            _rigidbody = GetComponent<Rigidbody2D>(); 
            _animator = GetComponent<Animator>();
        }

        private void FixedUpdate()
        {
            if (Input.GetKey(KeyCode.RightArrow))
            {
                _rigidbody.angularVelocity = -1 * _rotationSpeed;
                _animator.SetBool(RightTurnKey, true);
            }
            else if (Input.GetKey(KeyCode.LeftArrow))
            {
                _rigidbody.angularVelocity = 1 * _rotationSpeed;
                _animator.SetBool(LeftTurnKey, true);
            }

            //if (rotation > 0)
            //{
            //    _rigidbody.angularVelocity = rotation * _rotationSpeed;
            //    _animator.SetBool(RightTurnKey, true);
            //}
            //else if (rotation < 0)
            //{
            //    _rigidbody.angularVelocity = -rotation * _rotationSpeed;
            //    _animator.SetBool(LeftTurnKey, true);
            //}

            else
            {
                _animator.SetBool(LeftTurnKey, false);
                _animator.SetBool(RightTurnKey, false);
            }

            if (burst > 0)
            {
                Vector2 burstDelta = transform.up * _burstSpeed;
                _rigidbody.velocity += burstDelta;
            }
        }
    }
}

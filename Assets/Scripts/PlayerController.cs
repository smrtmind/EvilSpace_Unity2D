using UnityEngine;

namespace Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _rotationSpeed = 150;
        [SerializeField] private float _burstSpeed = 1;
        [SerializeField] private GameObject _leftStarterFlame;
        [SerializeField] private GameObject _rightStarterFlame;

        private Animator _animator;

        private readonly int LeftStarterKey = Animator.StringToHash("left-turn");
        private readonly int RightStarterKey = Animator.StringToHash("right-turn");

        public float rotation { get; set; }
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
            var isLeftPressed = Input.GetKey(KeyCode.LeftArrow);
            var isRightPressed = Input.GetKey(KeyCode.RightArrow);
            var isMovingForward = burst > 0;

            if (isLeftPressed)
            {
                _rigidbody.angularVelocity = 1 * _rotationSpeed;

                PlayAnimation(LeftStarterKey);
                ActivateObject(_rightStarterFlame);
            }
            else if (isRightPressed)
            {
                _rigidbody.angularVelocity = -1 * _rotationSpeed;

                PlayAnimation(RightStarterKey);
                ActivateObject(_leftStarterFlame);
            }
            else
            {
                PlayAnimation(disableAll: true);
                ActivateObject(disableAll: true);
            }

            if (isMovingForward)
            {
                Vector2 burstDelta = transform.up * _burstSpeed;
                _rigidbody.velocity += burstDelta;

                ActivateObject(disableAll: false);
            }
        }

        private void PlayAnimation(int animation = default, bool disableAll = false)
        {
            _animator.SetBool(LeftStarterKey, false);
            _animator.SetBool(RightStarterKey, false);

            if (!disableAll)
                _animator.SetBool(animation, true);
        }

        private void ActivateObject(GameObject go = null, bool disableAll = false)
        {
            SetGeneralState(false);

            if (go != null)
                go.SetActive(true);
            else
                SetGeneralState(disableAll ? false : true);

            void SetGeneralState(bool state)
            {
                _leftStarterFlame.SetActive(state);
                _rightStarterFlame.SetActive(state);
            }
        }
    }
}

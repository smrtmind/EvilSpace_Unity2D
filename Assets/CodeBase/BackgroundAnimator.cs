using UnityEngine;

namespace Scripts
{
    public class BackgroundAnimator : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _widthOverBorder = 100f;

        private Transform _background;
        private float _defaultPositonX;
        private bool _stop;

        private void Start()
        {
            _background = GetComponent<Transform>();
            _defaultPositonX = _background.transform.position.x;
        }

        private void FixedUpdate()
        {
            if (!_stop)
            {
                _background.transform.position = new Vector2(transform.position.x + _speed, transform.position.y);

                if (_background.transform.position.x > _widthOverBorder)
                {
                    _background.transform.position = new Vector2(_defaultPositonX, transform.position.y);
                }
            }
        }

        public void StopBackgroundAnimation() => _stop = true;

        public void StartBackgroundAnimation() => _stop = false;
    }
}

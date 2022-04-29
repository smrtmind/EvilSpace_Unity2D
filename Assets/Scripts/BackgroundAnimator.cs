using UnityEngine;

namespace Scripts
{
    public class BackgroundAnimator : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _widthOverBorder = 100f;

        private Transform _background;
        private float _defaultPositonX;

        private void Start()
        {
            _background = GetComponent<Transform>();
            _defaultPositonX = _background.transform.position.x;
        }

        private void Update()
        {
            _background.transform.position = new Vector2(transform.position.x + _speed, transform.position.y);

            if (_background.transform.position.x > _widthOverBorder)
            {               
                _background.transform.position = new Vector2(_defaultPositonX, transform.position.y);
            }
        }
    }
}

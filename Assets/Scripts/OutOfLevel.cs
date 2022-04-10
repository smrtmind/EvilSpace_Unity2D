using UnityEngine;

namespace Scripts
{
    public class OutOfLevel : MonoBehaviour
    {
        private Bounds _screenBounds;

        private void Start()
        {
            var boundsComponent = FindObjectOfType<ScreenBounds>();
            _screenBounds = boundsComponent.Bounds;
        }

        private void Update()
        {
            if (transform.position.x > _screenBounds.max.x)
                transform.position = new Vector3(_screenBounds.min.x, transform.position.y);

            if (transform.position.x < _screenBounds.min.x)
                transform.position = new Vector3(_screenBounds.max.x, transform.position.y);

            if (transform.position.y > _screenBounds.max.y)
                transform.position = new Vector3(transform.position.x, _screenBounds.min.y);

            if (transform.position.y < _screenBounds.min.y)
                transform.position = new Vector3(transform.position.x, _screenBounds.max.y);
        }
    }
}

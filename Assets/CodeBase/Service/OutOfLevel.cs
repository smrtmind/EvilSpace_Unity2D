using CodeBase.Utils;
using UnityEngine;

namespace CodeBase.Service
{
    public class OutOfLevel : MonoBehaviour
    {
        private Bounds screenBounds;

        private void Start()
        {
            var boundsComponent = FindObjectOfType<ScreenBounds>();
            screenBounds = boundsComponent.Bounds;
        }

        private void Update()
        {
            if (transform.position.x > screenBounds.max.x)
                transform.position = new Vector3(screenBounds.min.x, transform.position.y);

            if (transform.position.x < screenBounds.min.x)
                transform.position = new Vector3(screenBounds.max.x, transform.position.y);

            if (transform.position.y > screenBounds.max.y)
                transform.position = new Vector3(transform.position.x, screenBounds.min.y);

            if (transform.position.y < screenBounds.min.y)
                transform.position = new Vector3(transform.position.x, screenBounds.max.y);
        }
    }
}

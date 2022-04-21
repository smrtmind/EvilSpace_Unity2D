using UnityEngine;

namespace Scripts
{
    public class ScreenBounds : MonoBehaviour
    {
        public Bounds Bounds { get; private set; }
        public Bounds borderOfBounds { get; private set; }

        private void Awake()
        {
            var mainCamera = FindObjectOfType<Camera>();

            var screenMin = mainCamera.ViewportToWorldPoint(Vector3.zero);
            var screenMax = mainCamera.ViewportToWorldPoint(new Vector3(1.1f, 1.1f, 1.1f));
            
            var center = mainCamera.transform.position;
            Bounds = InitializeBounds(center, screenMin, screenMax);

            var border = Vector3.one;
            borderOfBounds = InitializeBounds(center, screenMax - border, screenMin + border);
        }

        private Bounds InitializeBounds(Vector3 center, Vector3 min, Vector3 max)
        {
            return new Bounds(center, new Vector3(max.x - min.x, max.y - min.y));
        }
    }
}

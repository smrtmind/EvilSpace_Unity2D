using UnityEngine;

namespace CodeBase.Utils
{
    public class ScreenBounds : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        public Bounds Bounds { get; private set; }
        public Bounds borderOfBounds { get; private set; }

        private Camera mainCamera;

        private void OnEnable()
        {
            mainCamera = dependencyContainer.MainCamera;
            dependencyContainer.ScreenBounds = this;

            Vector3 screenMin = mainCamera.ViewportToWorldPoint(Vector3.zero);
            Vector3 screenMax = mainCamera.ViewportToWorldPoint(Vector3.one);

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

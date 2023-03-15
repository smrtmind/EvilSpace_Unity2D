using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Utils
{
    public class ScreenBounds : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        public Bounds Bounds { get; private set; }
        public Bounds borderOfBounds { get; private set; }

        public static Action<Bounds> OnScreenBoundsInitializated;

        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = dependencyContainer.MainCamera;

            var screenMin = mainCamera.ViewportToWorldPoint(Vector3.zero);
            Vector3 screenMax; 

            var scene = SceneManager.GetActiveScene().name;
            if (scene == "MainMenu")
            {
                screenMax = mainCamera.ViewportToWorldPoint(new Vector3(1.5f, 1.5f, 1.5f));
            }
            else
            {
                screenMax = mainCamera.ViewportToWorldPoint(new Vector3(1.03f, 1.03f, 1.03f));
            }
            
            
            var center = mainCamera.transform.position;
            Bounds = InitializeBounds(center, screenMin, screenMax);

            var border = Vector3.one;
            borderOfBounds = InitializeBounds(center, screenMax - border, screenMin + border);

            OnScreenBoundsInitializated?.Invoke(borderOfBounds);
        }

        private Bounds InitializeBounds(Vector3 center, Vector3 min, Vector3 max)
        {
            return new Bounds(center, new Vector3(max.x - min.x, max.y - min.y));
        }
    }
}

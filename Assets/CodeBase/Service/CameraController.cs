using CodeBase.Utils;
using UnityEngine;

namespace CodeBase.Service
{
    public class CameraController : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        private void Awake()
        {
            dependencyContainer.MainCamera = Camera.main;
        }
    }
}

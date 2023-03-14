using UnityEngine;

namespace CodeBase.Utils
{
    public class Pool : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        [field: Header("Containers")]
        [field: SerializeField] public Transform ProjectileContainer { get; private set; }
        [field: SerializeField] public Transform EnemyContainer { get; private set; }

        private void Awake()
        {
            dependencyContainer.Pool = this;
        }
    }
}

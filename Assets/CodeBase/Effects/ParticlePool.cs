using CodeBase.ObjectBased;
using CodeBase.Utils;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Effects
{
    public class ParticlePool : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        [field: Header("Containers")]

        [field: Space]
        [field: Header("Projectiles")]
        [field: SerializeField] public Transform ProjectileContainer { get; private set; }
        [field: SerializeField] public List<Projectile> ProjectilesPool { get; private set; }

        [field: Space]
        [field: SerializeField] public Transform EnemyContainer { get; private set; }
        [field: SerializeField] public Transform ParticleContainer { get; private set; }
        [field: SerializeField] public Transform PopUpContainer { get; private set; }

        [Space]
        [SerializeField] private List<ParticleObject> objects = new List<ParticleObject>();

        private void Awake()
        {
            dependencyContainer.ParticlePool = this;
        }

        public ParticleObject GetFreeObject(ParticleType type)
        {
            ParticleObject freeObject = objects.Find(obj => obj.Type == type && !obj.IsBusy);
            if (freeObject == null)
                freeObject = CreateNewObject(type);

            return freeObject;
        }

        private ParticleObject CreateNewObject(ParticleType type)
        {
            ParticleObject newObject = Instantiate(objects.Find(obj => obj.Type == type), ParticleContainer);
            objects.Add(newObject);

            return newObject;
        }
    }
}

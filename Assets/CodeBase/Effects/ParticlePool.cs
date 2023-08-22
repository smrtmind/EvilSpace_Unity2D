using CodeBase.ObjectBased;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Effects
{
    public class ParticlePool : MonoBehaviour
    {
        [field: Header("Projectiles")]
        [field: SerializeField] public Transform ProjectileContainer { get; private set; }
        [field: SerializeField] public List<Projectile> ProjectilesPool { get; private set; }

        [field: Header("Containers")]
        [field: SerializeField] public Transform EnemyContainer { get; private set; }
        [field: SerializeField] public Transform ParticleContainer { get; private set; }
        [field: SerializeField] public Transform PopUpContainer { get; private set; }
        [field: SerializeField] public Transform PlanetContainer { get; private set; }

        [Space]
        [SerializeField] private List<ParticleObject> objects = new List<ParticleObject>();

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

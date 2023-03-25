using System;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Mobs
{
    [CreateAssetMenu(fileName = "EnemyStorage", menuName = "ScriptableObjects/EnemyStorage")]
    public class EnemyStorage : ScriptableObject
    {
        [field: SerializeField] public float DamageOnCollision { get; private set; }

        [Space]
        [SerializeField] private List<ObjectsGroup> ObjectGroups;

        public List<Enemy> GetEnemyUnits(ObjectType enemyClass)
        {
            foreach (var group in ObjectGroups)
            {
                if (group.Type == enemyClass)
                {
                    return group.Objects;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class ObjectsGroup
    {
        [field: SerializeField] public ObjectType Type { get; private set; }
        [field: SerializeField] public List<Enemy> Objects { get; private set; }
    }
}

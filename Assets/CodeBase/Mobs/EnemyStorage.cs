using System;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Mobs
{
    [CreateAssetMenu(fileName = "EnemyStorage", menuName = "ScriptableObjects/EnemyStorage")]
    public class EnemyStorage : ScriptableObject
    {
        [SerializeField] private List<EnemyUnit> Enemies;

        public EnemyUnit GetEnemyUnit(EnemyType type)
        {
            foreach (EnemyUnit unit in Enemies)
            {
                if (unit.Type == type)
                {
                    return unit;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class EnemyUnit
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public List<Enemy> Prefabs { get; private set; }
    }
}

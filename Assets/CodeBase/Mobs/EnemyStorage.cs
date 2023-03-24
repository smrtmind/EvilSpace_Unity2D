using System;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Mobs
{
    [CreateAssetMenu(fileName = "EnemyStorage", menuName = "ScriptableObjects/EnemyStorage")]
    public class EnemyStorage : ScriptableObject
    {
        [SerializeField] private List<EnemyUnitData> Enemies;

        public EnemyUnitData GetEnemyUnitData(EnemyType type)
        {
            foreach (EnemyUnitData unitData in Enemies)
            {
                if (unitData.Type == type)
                {
                    return unitData;
                }
            }

            return null;
        }
    }

    [Serializable]
    public class EnemyUnitData
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Score { get; private set; }
        [field: SerializeField] public List<Enemy> Prefabs { get; private set; }
    }
}

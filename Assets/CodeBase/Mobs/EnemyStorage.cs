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
    }

    [Serializable]
    public class EnemyUnit
    {
        [field: SerializeField] public EnemyType Type { get; private set; }
        [field: SerializeField] public Enemy Prefab { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
    }
}

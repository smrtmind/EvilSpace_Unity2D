using System;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.ObjectBased
{
    [CreateAssetMenu(fileName = "WeaponStorage", menuName = "ScriptableObjects/WeaponStorage")]
    public class WeaponStorage : ScriptableObject
    {
        [SerializeField] private List<WeaponData> playerWeapons;
        [SerializeField] private List<WeaponData> enemyWeapons;

        public WeaponData GetPlayerWeapon(WeaponType type)
        {
            foreach (WeaponData weapon in playerWeapons)
                if (type == weapon.WeaponType)
                    return weapon;

            return null;
        }

        public WeaponData GetEnemyWeapon(WeaponType type)
        {
            foreach (WeaponData weapon in enemyWeapons)
                if (type == weapon.WeaponType)
                    return weapon;

            return null;
        }
    }

    [Serializable]
    public class WeaponData
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public Projectile Projectile { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
    }
}

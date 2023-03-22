using System;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.ObjectBased
{
    [CreateAssetMenu(fileName = "WeaponStorage", menuName = "ScriptableObjects/WeaponStorage")]
    public class WeaponStorage : ScriptableObject
    {
        [SerializeField] private List<Weapon> weapons;

        public Weapon GetCurrentWeapon(WeaponType type)
        {
            foreach (Weapon weapon in weapons)
                if (type == weapon.WeaponType)
                    return weapon;

            return null;
        }
    }

    [Serializable]
    public class Weapon
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [field: SerializeField] public Projectile Projectile { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
    }
}

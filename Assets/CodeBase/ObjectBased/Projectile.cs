using CodeBase.Utils;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.ObjectBased
{
    public class Projectile : BaseProjectile
    {
        [field: SerializeField] public PlayerWeaponType WeaponType { get; private set; }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Enemy))
                SetBusyState(false);
        }
    }
}

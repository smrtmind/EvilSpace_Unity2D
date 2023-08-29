using CodeBase.Utils;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.ObjectBased
{
    public class EnemyProjectile : BaseProjectile
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }

        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Player) || collision.gameObject.tag.Equals(Tags.Shield))
                SetBusyState(false);
        }
    }
}

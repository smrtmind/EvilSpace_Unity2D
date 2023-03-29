using CodeBase.Utils;
using UnityEngine;

namespace CodeBase.ObjectBased
{
    public class EnemyProjectile : Projectile
    {
        protected override void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Player) || collision.gameObject.tag.Equals(Tags.Shield))
            {
                SetBusyState(false);
            }
        }
    }
}

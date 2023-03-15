using CodeBase.Player;
using CodeBase.Utils;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Mobs
{
    public abstract class Enemy : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] protected PlayerStorage playerStorage;

        [field: Header("Parent Class Settings")]
        [field: SerializeField] public EnemyType EnemyType { get; private set; }
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float Damage { get; private set; }
        [field: SerializeField] public bool IsBusy { get; private set; }

        private float currentHealth;

        public void SetBusyState(bool state)
        {
            IsBusy = state;
            gameObject.SetActive(IsBusy);
        }

        public void ModifyHealth(float health)
        {
            currentHealth += health;
            if (currentHealth <= 0f)
            {
                currentHealth = Health;
                IsBusy = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Projectile))
            {
                SetBusyState(false);
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Projectile))
            {
                SetBusyState(false);
            }
        }
    }
}

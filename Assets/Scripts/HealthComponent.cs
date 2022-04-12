using Scripts;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public UnityEvent _onDie;
        [SerializeField] public HealthChangeEvent _onChange;
        [SerializeField] private Cooldown _hitCooldown;

        public int Health => _health;

        public void ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return;

            if (_hitCooldown.IsReady)
            {
                _health += healthDelta;
                _onChange?.Invoke(_health);

                _hitCooldown.Reset();
            }
            else return;

            if (healthDelta < 0)
            {
                _onDamage?.Invoke();
            }

            if (healthDelta > 0)
            {
                _onHeal?.Invoke();
            }

            if (_health <= 0)
            {
                _onDie?.Invoke();
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Update Health")]
        private void UpdateHealth()
        {
            _onChange?.Invoke(_health);
        }
#endif

        public void SetHealth(int health)
        {
            _health = health;
        }

        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }

        [Serializable]
        public class HealthChangeEvent : UnityEvent<int>
        {

        }
    }
}

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

        public int Health => _health;

        private int _maxHealth;

        private void Awake()
        {
            _maxHealth = _health;
        }

        public void ResetHealth()
        {
            _health = _maxHealth;
        }

        public void ModifyHealth(int healthDelta)
        {
            if (_health <= 0) return;
            _health += healthDelta;

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

        private void OnDestroy()
        {
            _onDie.RemoveAllListeners();
        }
    }
}

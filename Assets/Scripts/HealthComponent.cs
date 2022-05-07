using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _maxHealth;
        [SerializeField] private UnityEvent _onDamage;
        [SerializeField] private UnityEvent _onHeal;
        [SerializeField] public UnityEvent _onDie;

        public int Health => _health;

        private int _defaultHealth;
        private PlayerController _player;

        private void Awake()
        {
            _defaultHealth = _health;
        }

        private void Start()
        {
            _player = GetComponent<PlayerController>();
        }

        public void ResetHealth()
        {
            _health = _defaultHealth;
        }

        public void RiseMaxHealth()
        {
            if (_defaultHealth < _maxHealth)
                _defaultHealth++;

            ResetHealth();
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

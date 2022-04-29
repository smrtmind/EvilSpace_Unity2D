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
        private Animator _animator;
        private PlayerController _player;

        private void Awake()
        {
            _maxHealth = _health;
        }

        private void Start()
        {
            _animator = GetComponent<Animator>();
            _player = GetComponent<PlayerController>();
        }

        public void ResetHealth()
        {
            _health = _maxHealth;
        }

        public void RiseMaxHealth()
        {
            _player.RemoveVisualDamage();

            _maxHealth++;
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

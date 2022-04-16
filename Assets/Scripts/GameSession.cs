using UnityEngine;

namespace Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _score;
        [SerializeField] private HealthComponent _targetHp;

        public int Health => _health;
        public int Score => _score;

        private void Update()
        {
            _health = _targetHp.Health;
        }

        public void AddScore(int value)
        {
            _score += value;
        }
    }
}

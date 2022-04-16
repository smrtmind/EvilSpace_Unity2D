using UnityEngine;

namespace Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _tries;
        [SerializeField] private int _health;
        [SerializeField] private int _score;
        [SerializeField] private HealthComponent _targetHp;

        public int Tries => _tries;
        public int Health => _health;
        public int Score => _score;

        public void AddScore(int score)
        {
            _score += score;
        }

        public void ModifyTries(int tries)
        {
            _tries += tries;
        }

        private void Update()
        {
            _health = _targetHp.Health;
        }
    }
}

using UnityEngine;

namespace Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _health;
        [SerializeField] private int _score;

        public int Health => _health;
        public int Score => _score;

        public void AddScore(int score)
        {
            _score += score;
        }

        public void ModifyHealth(int healthDelta)
        {
            _health += healthDelta;
        }
    }
}

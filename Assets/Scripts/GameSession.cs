using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _tries;
        [SerializeField] private int _health;
        [SerializeField] private int _score;
        [SerializeField] private HealthComponent _targetHp;
        [SerializeField] private AudioClip _oneUp;

        public int Tries => _tries;
        public int Health => _health;
        public int Score => _score;

        private int stepToAddLife = 100;

        public void ModifyScore(int score)
        {
            _score += score;
        }

        public void ResetScore()
        {
            _score = 0;
        }

        public void ModifyTries(int tries)
        {
            _tries += tries;
        }

        private void Update()
        {
            _health = _targetHp.Health;

            if (_score >= stepToAddLife)
            {
                FindObjectOfType<AudioSource>().PlayOneShot(_oneUp);
                ModifyTries(1);
                _targetHp.RiseMaxHealth();
                stepToAddLife *= 2;
            }
        }
    }
}

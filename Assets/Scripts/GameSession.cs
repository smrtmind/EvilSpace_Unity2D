using UnityEngine;

namespace Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _tries;
        [SerializeField] private int _health;
        [SerializeField] private HealthComponent _targetHp;
        [SerializeField] private AudioClip _oneUp;
        [SerializeField] private SpawnComponent _levelUp;

        public int Tries => _tries;
        public int Health => _health;
        public int Score => _score;

        private int _nextLvl = 100;
        private int _xp;
        private int _score;
        private int _playerLvl = 1;

        public int NextLvl => _nextLvl;
        public int XP => _xp;
        public int PlayerLVL => _playerLvl;

        public void ModifyXp(int xp)
        {
            _score += xp;
            _xp += xp;
        }

        public void ModifyTries(int tries) => _tries += tries;

        private void Update()
        {
            _health = _targetHp.Health;

            if (_xp == _nextLvl)
            {
                LevelUp(0);
            }
            else if (_xp > _nextLvl)
            {
                LevelUp(_xp - _nextLvl);
            }
        }

        private void LevelUp(int currentXp)
        {
            FindObjectOfType<AudioSource>().PlayOneShot(_oneUp);
            _xp = currentXp;
            ModifyTries(1);
            _targetHp.RiseMaxHealth();
            _nextLvl *= 2;
            _playerLvl++;

            _levelUp.Spawn();
        }
    }
}

using UnityEngine;

namespace Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _tries;
        [SerializeField] private int _health;
        [SerializeField] private HealthComponent _targetHp;
        [SerializeField] private AudioClip _oneUp;

        [Space]
        [SerializeField] private GameObject[] _enemySpawners;

        private int _score;
        private int _xp;
        private int _playerLvl = 1;
        private int _nextLvl = 500;
        public bool _isLevelUp;

        public int Tries => _tries;
        public int Health => _health;
        public int Score => _score;
        public int XP => _xp;
        public int PlayerLVL => _playerLvl;
        public int NextLvl => _nextLvl;   

        public void ModifyXp(int xp)
        {
            _score += xp;
            _xp += xp;
        }

        public void ModifyTries(int tries)
        {
            _tries += tries;
            if (_tries < 0)
                _tries = 0;
        }

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
            _isLevelUp = true;

            FindObjectOfType<AudioSource>().PlayOneShot(_oneUp);

            FindObjectOfType<WeaponController>().PowerUp();             

            _playerLvl++;
            _xp = currentXp;
            _tries++;
            _targetHp.RiseMaxHealth();

            _nextLvl = (((_nextLvl / 100) * 20) + _nextLvl);

            PowerUpEnemies();
        }

        private void PowerUpEnemies()
        {
            switch (_playerLvl)
            {
                //small enemies
                case 2:
                    _enemySpawners[0].SetActive(true);
                    break;

                //medium enemies
                case 4:
                    _enemySpawners[1].SetActive(true);
                    break;

                //large enemies
                case 6:
                    _enemySpawners[2].SetActive(true);
                    break;
            }

            foreach (var spawner in _enemySpawners)
            {
                if (spawner.activeSelf)
                {
                    var enemyCooldown = spawner.GetComponent<EnemySpawner>().SpawnCooldown;
                    enemyCooldown.Value -= 1.0f;

                    if (enemyCooldown.Value <= 4.0f)
                        enemyCooldown.Value = 4.0f;
                }
            }
        }
    }
}

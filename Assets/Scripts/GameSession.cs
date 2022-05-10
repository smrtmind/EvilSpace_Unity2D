using UnityEngine;

namespace Scripts
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private int _tries;
        [SerializeField] private int _health;
        [SerializeField] private HealthComponent _targetHp;
        [SerializeField] private AudioSource _mainTheme;
        [SerializeField] private AudioSource _bossTheme;

        [Space]
        [SerializeField] private GameObject[] _enemySpawners;

        [Space]
        [Header("Boss")]
        [SerializeField] private SpawnComponent _bossSpawner;

        private static readonly int BossAttentionKey = Animator.StringToHash("bossAttention");

        private int _score;
        private int _xp;
        private int _playerLvl = 1;
        private int _nextLvl = 500;
        private Animator _bossAnimator;
        private PlayerController _player;
        private AudioSource _audio;
        private WeaponController _weaponController;

        public int Tries => _tries;
        public int Health => _health;
        public int Score => _score;
        public int XP => _xp;
        public int PlayerLVL => _playerLvl;
        public int NextLvl => _nextLvl;

        private void Awake()
        {
            _bossAnimator = GetComponent<Animator>();
            _player = FindObjectOfType<PlayerController>();
            _audio = GetComponent<AudioSource>();
            _weaponController = FindObjectOfType<WeaponController>();
        }

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

            if (_player.IsDead) return;

            if (_xp == _nextLvl)
            {
                LevelUp();
            }
            else if (_xp > _nextLvl)
            {
                LevelUp(_xp - _nextLvl);
            }
        }

        private void LevelUp(int currentXp = 0)
        {
            if (!_weaponController)
                _weaponController = FindObjectOfType<WeaponController>();

            if (!_player)
                _player = FindObjectOfType<PlayerController>();

            _weaponController.Shield.SetActive(true);
            _weaponController.Shield.GetComponent<TimerComponent>().SetTimer(0);
            _player._levelUpEffect.Spawn();

            _playerLvl++;
            _xp = currentXp;

            if (_playerLvl % 8 == 0)
            {              
                SetEnemySpawnersState(false);
                _weaponController.KillAllEnemies();
                _bossAnimator.SetTrigger(BossAttentionKey);
            }

            _player.RemoveVisualDamage();
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
                    var enemyCooldown = spawner.GetComponent<ObjectsSpawner>().SpawnCooldown;
                    enemyCooldown.Value -= 1.0f;

                    if (enemyCooldown.Value <= 2.0f)
                        enemyCooldown.Value = 2.0f;
                }
            }
        }

        public void PlayArrivalSFX()
        {
            _audio.Play();
        }

        public void SpawnBoss()
        {
            _bossSpawner.Spawn();
            _mainTheme.Pause();
            _bossTheme.Play();
        }

        public void ReturnMainTheme()
        {
            _bossTheme.Stop();
            _mainTheme.Play();
        }

        private void SetEnemySpawnersState(bool state)
        {
            foreach (var enemy in _enemySpawners)
            {
                enemy.GetComponent<ObjectsSpawner>().SetState(state);
            }
        }
    }
}

using CodeBase.Player;
using CodeBase.UI;
using Scripts;
using System;
using UnityEngine;

namespace CodeBase.Service
{
    public class GameSession : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private PlayerStorage playerStorage;

        [SerializeField] private int _tries;
        [SerializeField] private TimerComponent _timers;

        [Space]
        [SerializeField] private float _minEnemySpawnCooldown;
        [SerializeField] private EnemySpawner[] _enemySpawners;

        [Space]
        [SerializeField] private SpawnComponent _bossSpawner;
        [SerializeField] private int _everyLevelSpawn;

        private static readonly int WarningKey = Animator.StringToHash("warning");

        private int _score;
        private int _xp;
        private int _playerLvl = 1;
        private int _nextLvl = 500;
        private PlayerController _player;
        private WeaponController _weaponController;
        private AudioComponent _audio;
        private HealthComponent _playerHealth;
        private UserInterface _hud;

        public int Tries => _tries;
        public HealthComponent PlayerHealth => _playerHealth;
        public int Score => _score;
        public int XP => _xp;
        public int PlayerLVL => _playerLvl;
        public int NextLvl => _nextLvl;

        private void Awake()
        {
            _player = FindObjectOfType<PlayerController>();
            _weaponController = FindObjectOfType<WeaponController>();
            _audio = FindObjectOfType<AudioComponent>();
            _playerHealth = _player.GetComponent<HealthComponent>();
            _hud = FindObjectOfType<UserInterface>();
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
            if (playerStorage.ConcretePlayer.IsDead) return;

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

            if (_playerLvl % _everyLevelSpawn == 0)
            {
                _weaponController.KillAllEnemies();
                DisableEnemySpawners();
                //_hud.Warning.SetTrigger(WarningKey);
                _timers.SetTimerByName("spawn boss");
            }

            _player.RemoveVisualDamage();
            _playerHealth.RiseMaxHealth();

            _nextLvl = (((_nextLvl / 100) * 20) + _nextLvl);

            IncreaseDifficulty();
        }

        private void IncreaseDifficulty()
        {
            foreach (var enemy in _enemySpawners)
            {
                if (_playerLvl >= enemy.LevelToStartSpawn)
                {
                    enemy.Spawner.enabled = true;
                }

                if (enemy.Spawner.enabled)
                {
                    var enemyCooldown = enemy.Spawner.SpawnCooldown;
                    enemyCooldown -= 1.0f;

                    if (enemyCooldown <= _minEnemySpawnCooldown)
                        enemyCooldown = _minEnemySpawnCooldown;
                }
            }
        }

        public void SpawnBoss()
        {
            _bossSpawner.Spawn();
            _audio.Play("boss fight");
        }

        private void DisableEnemySpawners()
        {
            foreach (var enemy in _enemySpawners)
            {
                enemy.Spawner.SetState(false);
            }
        }

        public void EnableEnemySpawners()
        {
            foreach (var enemy in _enemySpawners)
            {
                if (_playerLvl >= enemy.LevelToStartSpawn)
                {
                    enemy.Spawner.SetState(true);
                }
            }
        }

        public void RestoreEnemies()
        {
            _timers.SetTimerByName("enemy spawners");

            _audio.Stop();
            _audio.PlayMainSource();
        }
    }

    [Serializable]
    public class EnemySpawner
    {
        [SerializeField] private ObjectsSpawner _spawner;
        [SerializeField] private int _levelToStartSpawn;

        public ObjectsSpawner Spawner => _spawner;
        public int LevelToStartSpawn => _levelToStartSpawn;
    }
}

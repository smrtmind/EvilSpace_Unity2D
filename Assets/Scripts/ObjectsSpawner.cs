using UnityEngine;

namespace Scripts
{
    public class ObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private int _enemiesOnStart;
        [SerializeField] private Cooldown _spawnCooldown;
        [SerializeField] private bool _startSpawn;

        public bool StartSpawn => _startSpawn;
        public Cooldown SpawnCooldown => _spawnCooldown;

        private Bounds _screenBounds;

        private void Start()
        {
            _screenBounds = FindObjectOfType<ScreenBounds>().borderOfBounds;

            if (_startSpawn)
            {
                SpawnEnemies(_enemiesOnStart);
                _spawnCooldown.Reset();
            } 
        }

        private void Update()
        {
            if (_startSpawn)
            {
                if (_spawnCooldown.IsReady)
                {
                    SpawnNewEnemy();
                    _spawnCooldown.Reset();
                }
            }
        }

        private void SpawnEnemies(int count)
        {
            for (int i = 0; i < count; i++)
                SpawnNewEnemy();
        }

        private void SpawnNewEnemy()
        {
            var randomEnemyIndex = Random.Range(0, _enemies.Length);
            var enemyPrefab = _enemies[randomEnemyIndex];

            var yPosition = Random.value > 0.5 ? _screenBounds.min.y : _screenBounds.max.y;
            var xPosition = Random.Range(_screenBounds.min.x, _screenBounds.max.x);

            var randomSpawnPosition = new Vector3(xPosition, yPosition);

            var asteroid = enemyPrefab.GetComponent<Asteroid>();
            if (asteroid)
            {
                Instantiate(asteroid, randomSpawnPosition, Quaternion.identity, transform).Launch();
            }
            else
                Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity, transform);
        }

        public void SetState(bool state)
        {
            _startSpawn = state;
        }
    }
}

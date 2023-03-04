using CodeBase.Mobs;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts
{
    public class ObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private GameObject[] _enemies;
        [SerializeField] private int objectsOnStart;
        [SerializeField] private bool _startSpawn;
        [field: SerializeField] public float SpawnCooldown { get; set; } = 5f;

        public bool StartSpawn => _startSpawn;

        private Bounds _screenBounds;

        private void Start()
        {
            _screenBounds = FindObjectOfType<ScreenBounds>().borderOfBounds;

            BurstSpawnObjects(objectsOnStart);
        }

        //private void Update()
        //{
        //    if (_startSpawn)
        //    {
        //        if (_spawnCooldown.IsReady)
        //        {
        //            SpawnNewEnemy();
        //            _spawnCooldown.Reset();
        //        }
        //    }
        //}

        private void BurstSpawnObjects(int count)
        {
            if (objectsOnStart > 0)
            {
                for (int i = 0; i < count; i++)
                    SpawnNewObject();
            }

            SpawnObjects();
        }

        private void SpawnNewObject()
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

        private async void SpawnObjects()
        {
            while (StartSpawn)
            {
                await Task.Delay((int)SpawnCooldown * 1000);

                SpawnNewObject();
            }
        }
    }
}

using UnityEngine;

namespace Scripts
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [SerializeField] private Asteroid[] _asteroids;
        [SerializeField] private int _initialAsteroids;
        [SerializeField] private Cooldown _spawnCooldown;

        private Bounds _screenBounds;

        private void Start()
        {
            _screenBounds = FindObjectOfType<ScreenBounds>().borderOfBounds;
            SpawnAsteroids(_initialAsteroids);
        }

        private void Update()
        {
            if (_spawnCooldown.IsReady)
            {
                SpawnNewAsteroid();
                _spawnCooldown.Reset();
            }
        }

        private void SpawnAsteroids(int count)
        {
            for (int i = 0; i < count; i++)
                SpawnNewAsteroid();
        }

        private void SpawnNewAsteroid()
        {
            var randomAsteroidIndex = Random.Range(0, _asteroids.Length);
            var asteroidPrefab = _asteroids[randomAsteroidIndex];

            var yPosition = Random.value > 0.5 ? _screenBounds.min.y : _screenBounds.max.y;
            var xPosition = Random.Range(_screenBounds.min.x, _screenBounds.max.x);

            var randomSpawnPosition = new Vector3(xPosition, yPosition);

            var asteroid = Instantiate(asteroidPrefab, randomSpawnPosition, Quaternion.identity, transform);
            asteroid.Launch();
        }
    }
}

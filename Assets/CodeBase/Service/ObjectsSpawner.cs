using CodeBase.Mobs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class ObjectsSpawner : MonoBehaviour
    {
        [SerializeField] private List<EnemyUnit> enemies;
        [SerializeField, Range(0, 50)] private int spawnUnitsOnStart;

        [field: SerializeField] public float SpawnCooldown { get; set; } = 5f;

        private Bounds screenBounds;

        private void OnEnable()
        {
            screenBounds = FindObjectOfType<ScreenBounds>().borderOfBounds;

            BurstSpawnObjects();
        }

        private void BurstSpawnObjects()
        {
            if (spawnUnitsOnStart > 0)
            {
                for (int i = 0; i < spawnUnitsOnStart; i++)
                    SpawnNewObject();
            }

            StartCoroutine(SpawnObjects());
        }

        private IEnumerator SpawnObjects()
        {
            while (true)
            {
                yield return new WaitForSeconds(SpawnCooldown);

                SpawnNewObject();
            }
        }

        private void SpawnNewObject()
        {
            foreach (EnemyUnit unit in enemies)
            {
                var randomEnemyIndex = Random.Range(0, unit.Enemies.Count);
                var enemyPrefab = unit.Enemies[randomEnemyIndex];

                var yPosition = Random.value > 0.5 ? screenBounds.min.y : screenBounds.max.y;
                var xPosition = Random.Range(screenBounds.min.x, screenBounds.max.x);

                var randomSpawnPosition = new Vector3(xPosition, yPosition);

                switch (unit.EnemyType)
                {
                    case EnemyType.Asteroid:
                        Asteroid newAsteroid = (Asteroid)Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity, transform);
                        newAsteroid.Launch();
                        break;

                    case EnemyType.SmallShip:
                    case EnemyType.MediumShip:
                    case EnemyType.LargeShip:
                        Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity, transform);
                        break;
                }
            }              
        }
    }

    [Serializable]
    public class EnemyUnit
    {
        [field: SerializeField] public EnemyType EnemyType { get; private set; }
        [field: SerializeField] public List<Enemy> Enemies { get; private set; }
    }
}

using CodeBase.Mobs;
using CodeBase.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;
using Random = UnityEngine.Random;

namespace Scripts
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        [SerializeField] private List<EnemyUnit> enemies;

        private Bounds screenBounds;

        private void OnEnable()
        {
            screenBounds = dependencyContainer.ScreenBounds.borderOfBounds;

            BurstSpawnObjects();
            StartCoroutine(SpawnObjects());
        }

        private void BurstSpawnObjects()
        {
            foreach (EnemyUnit unit in enemies)
            {
                if (unit.SpawnUnitsOnStart > 0)
                {
                    for (int i = 0; i < unit.SpawnUnitsOnStart; i++)
                        SpawnNewObject();
                }
            }
        }

        private IEnumerator SpawnObjects()
        {
            while (true)
            {
                foreach(EnemyUnit unit in enemies)
                {
                    yield return new WaitForSeconds(unit.SpawnCooldown);

                    SpawnNewObject();
                }
            }
        }

        private void SpawnNewObject()
        {
            foreach (EnemyUnit unit in enemies)
            {
                int randomEnemyIndex = Random.Range(0, unit.Enemies.Count);
                Enemy enemyPrefab = unit.Enemies[randomEnemyIndex];

                float yPosition = Random.value > 0.5 ? screenBounds.min.y : screenBounds.max.y;
                float xPosition = Random.Range(screenBounds.min.x, screenBounds.max.x);

                Vector3 randomSpawnPosition = new Vector3(xPosition, yPosition);

                switch (unit.EnemyType)
                {
                    case EnemyType.Asteroid:
                        Asteroid newAsteroid = (Asteroid)Instantiate(enemyPrefab, randomSpawnPosition, Quaternion.identity, transform);
                        newAsteroid?.Launch();
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
        [field: SerializeField, Range(0, 50)] public int SpawnUnitsOnStart { get; private set; }
        [field: SerializeField] public float SpawnCooldown { get; private set; }
    }
}

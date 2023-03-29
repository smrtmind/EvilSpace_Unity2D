using CodeBase.Mobs;
using CodeBase.ObjectBased;
using CodeBase.UI;
using CodeBase.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CodeBase.Utils.Enums;
using Random = UnityEngine.Random;

namespace CodeBase.Service
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;
        [SerializeField] private EnemyStorage enemyStorage;

        [Space]
        [SerializeField] private List<SpawnParameters> enemies;

        private Bounds screenBounds;
        private List<Coroutine> spawnCoroutines = new List<Coroutine>();

        private void OnEnable()
        {
            UserInterface.OnLevelLoaded += InitSpawner;
            UserInterface.OnGameRestarted += DisableAllEnemies;
        }

        private void OnDisable()
        {
            UserInterface.OnLevelLoaded -= InitSpawner;
            UserInterface.OnGameRestarted -= DisableAllEnemies;
        }

        private void InitSpawner()
        {
            screenBounds = dependencyContainer.ScreenBounds.borderOfBounds;

            BurstSpawnEnemies();
            StartSpawnEnemies(true);
        }

        private void BurstSpawnEnemies()
        {
            foreach (SpawnParameters unit in enemies)
            {
                if (unit.SpawnUnitsOnStart > 0)
                {
                    for (int i = 0; i < unit.SpawnUnitsOnStart; i++)
                        SpawnNewObject(unit);
                }
            }
        }

        private IEnumerator SpawnEnemies(SpawnParameters unit)
        {
            while (true)
            {
                yield return new WaitForSeconds(unit.SpawnCooldown);

                SpawnNewObject(unit);
            }
        }

        private void SpawnNewObject(SpawnParameters unit)
        {
            float yPosition = screenBounds.min.y;/*Random.value > 0.5 ? screenBounds.min.y : screenBounds.max.y;*/
            float xPosition = Random.Range(screenBounds.min.x, screenBounds.max.x);

            Vector3 randomPosition = new Vector3(xPosition, yPosition);

            Enemy newEnemy = GetFreeEnemy(unit);
            newEnemy.transform.position = randomPosition;
            newEnemy.transform.rotation = Quaternion.identity;
            newEnemy.SetBusyState(true);           
        }

        public void StartSpawnEnemies(bool start)
        {
            if (start)
            {
                for (int i = 0; i < enemies.Count; i++)
                    spawnCoroutines.Add(StartCoroutine(SpawnEnemies(enemies[i])));
            }
            else
            {
                spawnCoroutines.ForEach(routine => StopCoroutine(routine));
            }
        }

        public Enemy GetFreeEnemy(SpawnParameters unit)
        {
            Enemy freeEnemy = unit.EnemiesPool.Find(enemy => !enemy.IsBusy);
            if (freeEnemy == null)
                freeEnemy = CreateNewEnemy(unit);

            return freeEnemy;
        }

        private Enemy CreateNewEnemy(SpawnParameters unit)
        {
            var enemies = enemyStorage.GetEnemyUnits(unit.Type);

            Enemy newEnemy = Instantiate(enemies[Random.Range(0, enemies.Count)], dependencyContainer.ParticlePool.EnemyContainer);
            unit.EnemiesPool.Add(newEnemy);
            Dictionaries.Enemies.Add(newEnemy.transform, newEnemy);

            return newEnemy;
        }

        private void DisableAllEnemies()
        {
            foreach (SpawnParameters unit in enemies)
            {
                foreach (Enemy enemy in unit.EnemiesPool)
                {
                    if (enemy.IsBusy)
                        enemy.SetBusyState(false);
                }
            }

            foreach (Coroutine enemySpawner in spawnCoroutines)
                StopCoroutine(enemySpawner);

            List<Projectile> projectiles = Dictionaries.Projectiles.Values.ToList();
            foreach (Projectile projectile in projectiles)
            {
                if (projectile.IsBusy)
                    projectile.SetBusyState(false);
            }
        }
    }

    [Serializable]
    public class SpawnParameters
    {
        [field: SerializeField] public ObjectType Type { get; private set; }
        [field: SerializeField, Range(0, 50)] public int SpawnUnitsOnStart { get; private set; }
        [field: SerializeField] public float SpawnCooldown { get; private set; }
        [field: SerializeField] public List<Enemy> EnemiesPool { get; private set; }
    }
}

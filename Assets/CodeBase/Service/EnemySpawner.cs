using CodeBase.Effects;
using CodeBase.Mobs;
using CodeBase.ObjectBased;
using CodeBase.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;
using static CodeBase.Utils.Enums;
using Random = UnityEngine.Random;

namespace CodeBase.Service
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private EnemyStorage enemyStorage;

        [Space]
        [SerializeField] private List<SpawnParameters> enemies;

        [Inject] private DiContainer diContainer;

        private ScreenBounds screenBounds;
        private Bounds bounds;
        private List<Coroutine> spawnCoroutines = new List<Coroutine>();
        private ParticlePool particlePool;

        [Inject]
        private void Construct(ScreenBounds bounds, ParticlePool pool)
        {
            screenBounds = bounds;
            particlePool = pool;
        }

        private void OnEnable()
        {
            EventObserver.OnLevelLoaded += InitSpawner;
            EventObserver.OnGameRestarted += DisableAllObjectsOnScreen;
            EventObserver.OnBombButtonPressed += DestroyAllEnemies;
        }

        private void OnDisable()
        {
            EventObserver.OnLevelLoaded -= InitSpawner;
            EventObserver.OnGameRestarted -= DisableAllObjectsOnScreen;
            EventObserver.OnBombButtonPressed -= DestroyAllEnemies;
        }

        private void InitSpawner()
        {
            bounds = screenBounds.borderOfBounds;

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
            float yPosition = bounds.min.y;/*Random.value > 0.5 ? screenBounds.min.y : screenBounds.max.y;*/
            float xPosition = Random.Range(bounds.min.x, bounds.max.x);

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
            var randomEnemy = enemies[Random.Range(0, enemies.Count)];

            Enemy newEnemy = diContainer.InstantiatePrefabForComponent<Enemy>(randomEnemy, particlePool.EnemyContainer);
            unit.EnemiesPool.Add(newEnemy);
            Dictionaries.Enemies.Add(newEnemy.transform, newEnemy);

            return newEnemy;
        }

        private void DisableAllObjectsOnScreen()
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

            DestroyEnemyParticles();
        }

        private void DestroyAllEnemies()
        {
            foreach (SpawnParameters unit in enemies)
            {
                foreach (Enemy enemy in unit.EnemiesPool)
                {
                    if (enemy.IsBusy)
                    {
                        enemy.ModifyHealth(-enemy.Health);
                        enemy.TakeScore();
                    }
                }
            }

            DestroyEnemyParticles();
        }

        private void DestroyEnemyParticles()
        {
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

using CodeBase.Mobs;
using CodeBase.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;
using Random = UnityEngine.Random;

namespace CodeBase.Service
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        [Space]
        [SerializeField] private List<EnemyUnit> enemies;

        private Bounds screenBounds;
        private List<Coroutine> spawnCoroutines = new List<Coroutine>();

        private void OnEnable()
        {
            ScreenBounds.OnScreenBoundsInitializated += Init;
        }

        private void OnDisable()
        {
            ScreenBounds.OnScreenBoundsInitializated -= Init;
        }

        private void Init(Bounds bounds)
        {
            screenBounds = bounds;

            BurstSpawnEnemies();
            StartSpawnEnemies(true);
        }

        //private void Start()
        //{
        //    screenBounds = dependencyContainer.ScreenBounds.borderOfBounds;

        //    BurstSpawnEnemies();
        //    StartSpawnEnemies(true);
        //}

        private void BurstSpawnEnemies()
        {
            foreach (EnemyUnit unit in enemies)
            {
                if (unit.SpawnUnitsOnStart > 0)
                {
                    for (int i = 0; i < unit.SpawnUnitsOnStart; i++)
                        SpawnNewObject(unit);
                }
            }
        }

        private IEnumerator SpawnEnemies(EnemyUnit unit)
        {
            while (true)
            {
                yield return new WaitForSeconds(unit.SpawnCooldown);

                SpawnNewObject(unit);
            }
        }

        private void SpawnNewObject(EnemyUnit unit)
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

        public Enemy GetFreeEnemy(EnemyUnit unit)
        {
            Enemy freeEnemy = unit.EnemiesPool.Find(enemy => !enemy.IsBusy);
            if (freeEnemy == null)
                freeEnemy = CreateNewEnemy(unit);

            return freeEnemy;
        }

        private Enemy CreateNewEnemy(EnemyUnit unit)
        {
            Enemy newEnemy = Instantiate(unit.Enemies[Random.Range(0, unit.Enemies.Count)], dependencyContainer.Pool.EnemyContainer);
            unit.EnemiesPool.Add(newEnemy);

            return newEnemy;
        }
    }

    [Serializable]
    public class EnemyUnit
    {
        [field: SerializeField] public EnemyType EnemyType { get; private set; }
        [field: SerializeField] public List<Enemy> Enemies { get; private set; }
        [field: SerializeField, Range(0, 50)] public int SpawnUnitsOnStart { get; private set; }
        [field: SerializeField] public float SpawnCooldown { get; private set; }
        [field: SerializeField] public List<Enemy> EnemiesPool { get; private set; }
    }
}

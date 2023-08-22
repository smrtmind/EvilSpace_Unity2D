using CodeBase.Effects;
using CodeBase.ObjectBased;
using CodeBase.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static CodeBase.Utils.Enums;

namespace CodeBase.Service
{
    public class PlanetSpawner : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private PlanetStorage planetStorage;

        [Space]
        [SerializeField] private float spawnCooldown;
        [SerializeField] private float indentY = 15f;
        [SerializeField] private float indentX = 5f;
        [SerializeField, Min(1f)] private float minSpeed;
        [SerializeField, Min(1f)] private float maxSpeed;

        [Inject] private DiContainer diContainer;

        private List<Planet> planets = new List<Planet>();
        private ScreenBounds screenBounds;
        private Bounds bounds;
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
        }

        private void OnDisable()
        {
            EventObserver.OnLevelLoaded -= InitSpawner;
        }

        private void InitSpawner()
        {
            bounds = screenBounds.borderOfBounds;
            StartCoroutine(SpawnEnemies());
        }

        private IEnumerator SpawnEnemies()
        {
            while (true)
            {
                SpawnNewPlanet();
                yield return new WaitForSeconds(spawnCooldown);
            }
        }

        private void SpawnNewPlanet()
        {
            float yPosition = bounds.min.y + indentY;
            float xPosition = Random.Range(bounds.min.x + indentX, bounds.max.x - indentX);

            Vector3 randomPosition = new Vector3(xPosition, yPosition);

            var newPlanet = GetFreePlanet();
            newPlanet.Take();
            newPlanet.transform.position = randomPosition;
            newPlanet.SetSpeed(Random.Range(minSpeed, maxSpeed));
            //newPlanet.transform.rotation = Quaternion.identity;
            //newEnemy.SetBusyState(true);
        }

        public Planet GetFreePlanet()
        {
            var randomPlanetType = planetStorage.GetRandomPlanetType();

            Planet freePlanet = planets.Find(planet => !planet.IsBusy && planet.Type == randomPlanetType);
            if (freePlanet == null)
                freePlanet = CreateNewPlanet(randomPlanetType);

            return freePlanet;
        }

        private Planet CreateNewPlanet(PlanetType type)
        {
            //var enemies = enemyStorage.GetEnemyUnits(unit.Type);
            //var randomEnemy = enemies[Random.Range(0, enemies.Count)];

            Planet newPlanet = diContainer.InstantiatePrefabForComponent<Planet>(planetStorage.GetPlanet(type), particlePool.PlanetContainer);
            planets.Add(newPlanet);

            return newPlanet;
        }
    }
}

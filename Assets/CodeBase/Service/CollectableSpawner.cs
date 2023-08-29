using CodeBase.Effects;
using CodeBase.ObjectBased;
using CodeBase.Utils;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;
using static CodeBase.Utils.Enums;

namespace CodeBase.Service
{
    public class CollectableSpawner : MonoBehaviour
    {
        [SerializeField] private Collectable[] collectables;
        [SerializeField] private int spawnOnStart;
        [SerializeField] private float spawnCooldown;

        private ScreenBounds screenBounds;
        private Bounds bounds;
        private ParticlePool particlePool;
        private Coroutine spawnRoutine;

        [Inject] private DiContainer diContainer;

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

            BurstSpawnCollectables();
            StartSpawnCollectables(true);
        }

        private void BurstSpawnCollectables()
        {
            if (spawnOnStart > 0)
            {
                for (int i = 0; i < spawnOnStart; i++)
                    SpawnNewCollectable();
            }
        }

        public void StartSpawnCollectables(bool start)
        {
            if (start)
            {
                spawnRoutine = StartCoroutine(SpawnCollectables());
            }
            else
            {
                StopCoroutine(spawnRoutine);
            }
        }

        private IEnumerator SpawnCollectables()
        {
            while (true)
            {
                yield return new WaitForSeconds(spawnCooldown);
                SpawnNewCollectable();
            }
        }

        private void SpawnNewCollectable()
        {
            var yPosition = bounds.min.y;
            var xPosition = Random.Range(bounds.min.x, bounds.max.x);
            var randomSpawnPosition = new Vector3(xPosition, yPosition);

            var collectable = GetFreeCollectable(collectables[Random.Range(0, collectables.Length)].Type);
            collectable.transform.position = randomSpawnPosition;
            collectable.transform.rotation = Quaternion.identity;
            collectable.Take();
        }

        public Collectable GetFreeCollectable(CollectableType type)
        {
            Collectable freeCollectable = particlePool.CollectablesPool.Find(collectable => !collectable.IsBusy && collectable.Type == type);
            if (freeCollectable == null)
                freeCollectable = CreateNewCollectable(type);

            return freeCollectable;
        }

        private Collectable CreateNewCollectable(CollectableType type)
        {
            var currentCollectable = collectables.FirstOrDefault(collectable => collectable.Type == type);

            Collectable newCollectable = diContainer.InstantiatePrefabForComponent<Collectable>(currentCollectable, particlePool.CollectableContainer);
            particlePool.CollectablesPool.Add(newCollectable);

            return newCollectable;
        }
    }
}

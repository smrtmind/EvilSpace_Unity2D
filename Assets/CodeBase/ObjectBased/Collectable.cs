using CodeBase.Utils;
using System.Collections;
using UnityEngine;
using Zenject;
using static CodeBase.Utils.Enums;
using Random = UnityEngine.Random;

namespace CodeBase.ObjectBased
{
    public class Collectable : MonoBehaviour
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public CollectableType Type { get; private set; }
        [field: SerializeField] public Color Color { get; private set; }
        [SerializeField] private Rigidbody2D collectableBody;
        [SerializeField] private float lifeSpan = 10f;
        [SerializeField] private float indentY = 5f;
        [SerializeField] private float indentX = 5f;
        [SerializeField, Range(1f, 10f)] private float minSpeed = 1f;
        [SerializeField, Range(1f, 10f)] private float maxSpeed = 5f;

        public bool IsBusy { get; private set; }

        private Bounds screenBounds;
        private Coroutine boundsCoroutine;
        private Coroutine lifeSpanRoutine;

        [Inject]
        private void Construct(ScreenBounds bounds)
        {
            screenBounds = bounds.Bounds;
        }

        private void OnEnable()
        {
            EventObserver.OnGameRestarted += Release;

            boundsCoroutine = StartCoroutine(CheckForScreenBounds());
            Launch();
        }

        private void OnDisable()
        {
            EventObserver.OnGameRestarted -= Release;

            StopCoroutine(boundsCoroutine);
        }

        private void SetBusyState(bool state)
        {
            IsBusy = state;
            gameObject.SetActive(IsBusy);
        }

        public void Take()
        {
            SetBusyState(true);
            lifeSpanRoutine = StartCoroutine(StartLifeSpanTimer());
        }

        private void Release()
        {
            StopCoroutine(lifeSpanRoutine);
            SetBusyState(false);
        }

        private IEnumerator StartLifeSpanTimer()
        {
            yield return new WaitForSeconds(lifeSpan);
            Release();
        }

        private IEnumerator CheckForScreenBounds()
        {
            while (true)
            {
                CheckScreenBoundaries();
                yield return new WaitForEndOfFrame();
            }
        }

        private void Launch()
        {
            float randomAngle = Random.Range(5f * Mathf.PI / 4f, 7f * Mathf.PI / 4f);
            Vector2 velocity = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle)) * Random.Range(minSpeed, maxSpeed);

            collectableBody.velocity = velocity;
        }

        //private void Launch()
        //{
        //    var randomDirection = Random.insideUnitCircle.normalized;
        //    collectableBody.velocity = randomDirection * Random.Range(minSpeed, maxSpeed);
        //}

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Player))
            {
                Release();
                EventObserver.OnCollectableGot?.Invoke(Type);
            }
        }

        private void CheckScreenBoundaries()
        {
            Vector3 newPosition = transform.position;

            if (newPosition.x > screenBounds.max.x - indentX || newPosition.x < screenBounds.min.x + indentX)
            {
                collectableBody.velocity = new Vector2(-collectableBody.velocity.x, collectableBody.velocity.y);
                newPosition.x = Mathf.Clamp(newPosition.x, screenBounds.min.x + indentX, screenBounds.max.x - indentX);
            }

            if (newPosition.y > screenBounds.max.y - indentY || newPosition.y < screenBounds.min.y + indentY)
            {
                collectableBody.velocity = new Vector2(collectableBody.velocity.x, -collectableBody.velocity.y);
                newPosition.y = Mathf.Clamp(newPosition.y, screenBounds.min.y + indentY, screenBounds.max.y - indentY);
            }

            transform.position = newPosition;
        }
    }
}

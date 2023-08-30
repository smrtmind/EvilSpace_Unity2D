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
            if (transform.position.x > screenBounds.max.x + indentX)
                transform.position = new Vector3(screenBounds.min.x - indentX, transform.position.y);

            if (transform.position.x < screenBounds.min.x - indentX)
                transform.position = new Vector3(screenBounds.max.x + indentX, transform.position.y);

            if (transform.position.y > screenBounds.max.y + indentY)
                transform.position = new Vector3(transform.position.x, screenBounds.min.y - indentY);

            if (transform.position.y < screenBounds.min.y - indentY)
                transform.position = new Vector3(transform.position.x, screenBounds.max.y + indentY);
        }
    }
}

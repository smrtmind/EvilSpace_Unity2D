using CodeBase.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.ObjectBased
{
    public class Projectile : MonoBehaviour, IAmAnimated
    {
        [field: SerializeField] public WeaponType WeaponType { get; private set; }
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed = 5f;

        [field: SerializeField] public SpriteRenderer Renderer { get; private set; }
        [field: SerializeField] public float DelayBetweenFrames { get; private set; }
        [field: SerializeField] public List<Sprite> Frames { get; private set; }

        public bool IsBusy { get; private set; }
        public float Damage { get; private set; }

        private Vector2 screenBoundaries;
        private Coroutine animationCoroutine;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            StartCoroutine(CheckForScreenBounds());
            animationCoroutine = StartCoroutine(StartAnimation());     
        }

        private void OnDisable()
        {
            StopCoroutine(animationCoroutine);
        }

        public void SetDamage(float damage) => Damage = damage;

        private IEnumerator StartAnimation()
        {
            while (true)
            {
                foreach (var frame in Frames)
                {
                    Renderer.sprite = frame;
                    yield return new WaitForSeconds(DelayBetweenFrames);
                }
            }
        }

        private IEnumerator CheckForScreenBounds()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                screenBoundaries = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

                if (transform.position.y > screenBoundaries.y || transform.position.y < -screenBoundaries.y
                || transform.position.x > screenBoundaries.x || transform.position.x < -screenBoundaries.x)
                {
                    SetBusyState(false);
                    break;
                }
            }
        }

        public void SetBusyState(bool isBusy)
        {
            IsBusy = isBusy;
            gameObject.SetActive(isBusy);
        }

        public void Launch(Vector2 velocity, Vector2 direction)
        {
            rb.velocity = velocity + direction * speed;
        }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Enemy))
            {
                SetBusyState(false);
            }
        }
    }
}

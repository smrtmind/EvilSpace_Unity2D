using CodeBase.Player;
using CodeBase.Utils;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CodeBase.ObjectBased
{
    public class Shield : MonoBehaviour
    {
        [field: SerializeField] public SpriteRenderer Renderer { get; private set; }
        [field: SerializeField] public float DelayBetweenFrames { get; private set; }
        [field: SerializeField] public List<Sprite> Frames { get; private set; }

        [Space]
        [SerializeField] private Vector3 maxScale;
        [SerializeField] private float transitionDelay;

        public bool IsActive { get; private set; }

        private Coroutine shieldCoroutine;
        private Coroutine animationCoroutine;
        private Sequence shieldBahaviour;
        private Vector3 defaultScale;
        private PlayerController playerController;

        [Inject]
        private void Construct(PlayerController player)
        {
            playerController = player;
        }

        private void Awake()
        {
            defaultScale = transform.localScale;
        }

        private void OnEnable()
        {
            transform.localScale = Vector3.zero;

            shieldBahaviour = DOTween.Sequence().SetAutoKill(true);
            shieldBahaviour.Append(transform.DOScale(maxScale, transitionDelay / 2f))
                           .Append(transform.DOScale(defaultScale, transitionDelay / 2f));

            shieldCoroutine = StartCoroutine(EnableShield());
            animationCoroutine = StartCoroutine(StartAnimation());
        }

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

        private IEnumerator EnableShield()
        {
            IsActive = true;

            yield return new WaitForSeconds(playerController.ElectroShieldActiveDuration);

            shieldBahaviour = DOTween.Sequence().SetAutoKill(true);
            shieldBahaviour.Append(transform.DOScale(maxScale, transitionDelay / 2f))
                           .Append(transform.DOScale(Vector3.zero, transitionDelay / 2f))
                           .OnComplete(() => DisableShield());
        }

        private void DisableShield()
        {
            StopCoroutine(shieldCoroutine);
            StopCoroutine(animationCoroutine);
            gameObject.SetActive(false);

            IsActive = false;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.tag.Equals(Tags.Enemy))
            {
                var enemy = Dictionaries.Enemies.FirstOrDefault(p => p.Key == collision.gameObject.transform);
                enemy.Value.ModifyHealth(-enemy.Value.Health);
                enemy.Value.TakeScore();
            }
        }
    }
}

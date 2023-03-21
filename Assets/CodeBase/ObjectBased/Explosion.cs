using CodeBase.Effects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.ObjectBased
{
    public class Explosion : ParticleObject, IAmAnimated
    {
        [field: SerializeField] public SpriteRenderer Renderer { get; private set; }
        [field: SerializeField] public float DelayBetweenFrames { get; private set; }
        [field: SerializeField] public List<Sprite> Frames { get; private set; }

        private Coroutine animationCoroutine;

        private void OnEnable()
        {
            animationCoroutine = StartCoroutine(StartAnimation());
        }

        private void OnDisable()
        {
            StopCoroutine(animationCoroutine);
        }

        private IEnumerator StartAnimation()
        {
            for (int i = 0; i < Frames.Count; i++)
            {
                Renderer.sprite = Frames[i];
                yield return new WaitForSeconds(DelayBetweenFrames);
            }

            SetBusyState(false);
        }
    }
}

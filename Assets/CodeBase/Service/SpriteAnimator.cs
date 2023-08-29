using CodeBase.Effects;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Service
{
    public class SpriteAnimator : ParticleObject
    {
        [SerializeField] private bool loop;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField][Range(1, 100)] private int frameRate = 10;
        [SerializeField] private List<Sprite> sprites;

        private float secondsPerFrame;
        private float nextFrameTime;
        private int currentFrame;

        private void OnEnable()
        {
            nextFrameTime = Time.time;
            currentFrame = default;
        }

        private void Start()
        {
            secondsPerFrame = 1f / frameRate;
        }

        private void Update()
        {
            if (nextFrameTime > Time.time) return;

            if (currentFrame >= sprites.Count)
            {
                if (loop)
                    currentFrame = default;
                else
                    SetBusyState(false);

                return;
            }

            spriteRenderer.sprite = sprites[currentFrame];
            nextFrameTime += secondsPerFrame;
            currentFrame++;
        }

        //[field: SerializeField] public SpriteRenderer Renderer { get; private set; }
        //[field: SerializeField] public float DelayBetweenFrames { get; private set; }
        //[field: SerializeField] public List<Sprite> Frames { get; private set; }

        //private Coroutine animationCoroutine;

        //private void OnEnable()
        //{
        //    animationCoroutine = StartCoroutine(StartAnimation());
        //}

        //private void OnDisable()
        //{
        //    StopCoroutine(animationCoroutine);
        //}

        //private IEnumerator StartAnimation()
        //{
        //    if (ParticleType == ParticleType.LargeShipExplosion)
        //    {
        //        while (true)
        //        {
        //            for (int i = 0; i < Frames.Count; i++)
        //            {
        //                Renderer.sprite = Frames[i];
        //                yield return new WaitForSeconds(DelayBetweenFrames);
        //            }
        //        }
        //    }
        //    else
        //    {
        //        for (int i = 0; i < Frames.Count; i++)
        //        {
        //            Renderer.sprite = Frames[i];
        //            yield return new WaitForSeconds(DelayBetweenFrames);
        //        }

        //        SetBusyState(false);
        //    }
        //}
    }
}


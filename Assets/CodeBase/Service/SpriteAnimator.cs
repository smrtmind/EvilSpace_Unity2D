using UnityEngine;

namespace CodeBase.Service
{
    public class SpriteAnimator : ImageAnimator
    {
        [SerializeField] private SpriteRenderer spriteRenderer;

        private void Update()
        {
            if (nextFrameTime > Time.time) return;

            if (currentFrame >= Sprites.Count)
            {
                if (loop) currentFrame = default;
                return;
            }

            spriteRenderer.sprite = Sprites[currentFrame];
            nextFrameTime += secondsPerFrame;
            currentFrame++;
        }
    }
}


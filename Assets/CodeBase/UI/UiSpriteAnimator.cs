using CodeBase.Service;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class UiSpriteAnimator : ImageAnimator
    {
        [SerializeField] private Image imageRenderer;

        private void Update()
        {
            if (nextFrameTime > Time.time) return;

            if (currentFrame >= Sprites.Count)
            {
                if (loop) currentFrame = default;
                return;
            }

            imageRenderer.sprite = Sprites[currentFrame];
            nextFrameTime += secondsPerFrame;
            currentFrame++;
        }
    }
}

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class CanvasImageAnimator : MonoBehaviour
    {
        [SerializeField] private Image imageRenderer;
        [SerializeField][Range(1, 100)] private int frameRate = 10;
        [SerializeField] private bool loop;
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
                if (loop) currentFrame = default;
                return;
            }

            imageRenderer.sprite = sprites[currentFrame];
            nextFrameTime += secondsPerFrame;
            currentFrame++;
        }
    }
}

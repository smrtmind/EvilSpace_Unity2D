using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Service
{
    [RequireComponent(typeof(SpriteRenderer))]

    public class SpriteAnimation : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] [Range(1, 100)] private int frameRate = 10;
        [SerializeField] private bool loop;

        [field: SerializeField] public List<Sprite> Sprites { get; private set; }

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


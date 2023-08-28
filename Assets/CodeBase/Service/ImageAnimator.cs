using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Service
{
    public class ImageAnimator : MonoBehaviour
    {
        [SerializeField][Range(1, 100)] protected int frameRate = 10;
        [SerializeField] protected bool loop;

        [field: SerializeField] protected List<Sprite> Sprites { get; private set; }

        protected float secondsPerFrame;
        protected float nextFrameTime;
        protected int currentFrame;

        private void OnEnable()
        {
            nextFrameTime = Time.time;
            currentFrame = default;
        }

        private void Start()
        {
            secondsPerFrame = 1f / frameRate;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.ObjectBased
{
    public interface IAmAnimated
    {
        public SpriteRenderer Renderer { get; }
        public float DelayBetweenFrames { get; }
        public List<Sprite> Frames { get; }
    }
}

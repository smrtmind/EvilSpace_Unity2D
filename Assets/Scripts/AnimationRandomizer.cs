using UnityEngine;

namespace Scripts
{
    public class AnimationRandomizer : MonoBehaviour
    {
        private SpriteAnimation _animation;

        private void Start()
        {
            _animation = GetComponent<SpriteAnimation>();
        }

        private void OnBecameVisible() => _animation.enabled = true;

        private void OnBecameInvisible() => _animation.enabled = false;
    }
}

using UnityEngine;

namespace Scripts
{
    public class WindowAnimation : MonoBehaviour
    {
        private Animator _animator;
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int LaunchGame = Animator.StringToHash("start");
        private static readonly int Escape = Animator.StringToHash("exit");

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();

            _animator.SetTrigger(Show);
        }

        public void StartGame()
        {
            _animator.SetTrigger(LaunchGame);
        }

        public void Exit()
        {
            _animator.SetTrigger(Escape);
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}

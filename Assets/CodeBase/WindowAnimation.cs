using UnityEngine;

namespace Scripts
{
    public class WindowAnimation : MonoBehaviour
    {
        [SerializeField] private GameObject _loading;
        [SerializeField] private AudioSource _ambient;

        private Animator _animator;
        private static readonly int RedButton = Animator.StringToHash("redButtonPressed");
        private static readonly int Show = Animator.StringToHash("show");
        private static readonly int LaunchGame = Animator.StringToHash("start");

        protected virtual void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void RedButtonPressed()
        {
            _animator.SetTrigger(RedButton);
        }

        public void ShowOptions()
        {
            _animator.SetTrigger(Show);
        }

        public void StartGame()
        {
            _animator.SetTrigger(LaunchGame);
        }

        public void LoadingScreen()
        {
            _loading.SetActive(true);
            _ambient.Stop();
        }

        public virtual void OnCloseAnimationComplete()
        {
            Destroy(gameObject);
        }
    }
}

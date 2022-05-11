using UnityEngine;

namespace Scripts
{
    public class PauseOverlay : MonoBehaviour
    {
        private BackgroundAnimator[] _backgroundAnimators;

        private void Awake()
        {
            _backgroundAnimators = FindObjectsOfType<BackgroundAnimator>();
        }

        private void OnEnable()
        {
            Time.timeScale = 0;

            foreach (var background in _backgroundAnimators)
            {
                background.StopBackgroundAnimation();
            }
        }

        private void OnDisable()
        {
            Time.timeScale = 1;

            foreach (var background in _backgroundAnimators)
            {
                background.StartBackgroundAnimation();
            }
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class GameOverOverlay : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        [SerializeField] private AudioSource _gameOverSound;

        private void OnEnable()
        {
            var session = FindObjectOfType<GameSession>();
            _scoreText.text = $"Final score: {session.Score}";

            _gameOverSound.Play();

            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class GameOverOverlay : MonoBehaviour
    {
        [SerializeField] private Text _scoreText;
        private AudioComponent _audio;

        private void Awake()
        {
            _audio = FindObjectOfType<AudioComponent>();
        }

        private void OnEnable()
        {
            var session = FindObjectOfType<GameSession>();
            _scoreText.text = $"FINAL SCORE: {session.Score} / LVL {session.PlayerLVL}";

            _audio.Play("game over", 0.2f);

            Time.timeScale = 0;
        }

        private void OnDestroy()
        {
            Time.timeScale = 1;
        }
    }
}

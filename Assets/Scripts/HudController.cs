using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Text _triesText;
        [SerializeField] private Slider _healthBar;
        [SerializeField] private Text _scoreText;

        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        private void FixedUpdate()
        {
            _healthBar.value = _gameSession.Health;
            _scoreText.text = _gameSession.Score.ToString();
            _triesText.text = $"x {_gameSession.Tries}";
        }
    }
}

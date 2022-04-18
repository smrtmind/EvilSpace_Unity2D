using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Text _triesText;
        [SerializeField] public Slider _healthBar;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _healthAmount;

        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        private void FixedUpdate()
        {
            _healthBar.value = _gameSession.Health;
            _scoreText.text = $"Score: {_gameSession.Score}";
            _triesText.text = $"x {_gameSession.Tries}";
            _healthAmount.text = $"{_healthBar.value}";
        }
    }
}

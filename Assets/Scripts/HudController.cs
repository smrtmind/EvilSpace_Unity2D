using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Text _triesText;
        [SerializeField] public Slider _XpBar;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _lvlText;
        [SerializeField] private Text _healthAmount;
        [SerializeField] private Text _levelProgress;

        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        private void Update()
        {
            _XpBar.value = _gameSession.XP;
            _XpBar.maxValue = _gameSession.NextLvl;

            _levelProgress.text = $"{_XpBar.value} / {_XpBar.maxValue}";

            _scoreText.text = $"SCORE: {_gameSession.Score}";
            _triesText.text = $"{_gameSession.Tries}";

            if (_gameSession.Health < 0)
                _healthAmount.text = "0";
            else
                _healthAmount.text = $"{_gameSession.Health}";

            _lvlText.text = $"LVL {_gameSession.PlayerLVL}";
        }
    }
}

using UnityEngine;
using UnityEngine.UI;

namespace Scripts
{
    public class HudController : MonoBehaviour
    {
        [SerializeField] private Text _triesText;
        [SerializeField] public Slider _HpBar;
        [SerializeField] public Slider _XpBar;
        [SerializeField] private Text _scoreText;
        [SerializeField] private Text _lvlText;
        [SerializeField] private Text _healthAmount;

        private GameSession _gameSession;

        private void Start()
        {
            _gameSession = FindObjectOfType<GameSession>();
        }

        private void Update()
        {
            _XpBar.value = _gameSession.XP;
            _XpBar.maxValue = _gameSession.NextLvl;

            _HpBar.value = _gameSession.Health;
            _scoreText.text = $"SCORE: {_gameSession.Score}";
            _triesText.text = $"x {_gameSession.Tries}";
            _healthAmount.text = $"{_HpBar.value}";
            _lvlText.text = $"LVL {_gameSession.PlayerLVL}";
        }
    }
}

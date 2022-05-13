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
        [SerializeField] private Animator _warning;

        [Space]
        [Header("WeaponStatus")]
        [SerializeField] private Text _gunValue;
        [SerializeField] private Text _blasterValue;
        [SerializeField] private Text _bombStatus;

        public Animator Warning => _warning;

        private GameSession _gameSession;
        private WeaponController _weaponController;

        private void Awake()
        {
            _gameSession = FindObjectOfType<GameSession>();
            _weaponController = FindObjectOfType<WeaponController>();
        }

        private void Update()
        {
            _XpBar.value = _gameSession.XP;
            _XpBar.maxValue = _gameSession.NextLvl;

            _levelProgress.text = $"{_XpBar.value} / {_XpBar.maxValue}";

            _scoreText.text = $"SCORE: {_gameSession.Score}";
            _triesText.text = $"{_gameSession.Tries}";

            if (_gameSession.PlayerHealth.Health < 0)
                _healthAmount.text = "0";
            else
                _healthAmount.text = $"{_gameSession.PlayerHealth.Health}";

            _lvlText.text = $"LVL {_gameSession.PlayerLVL}";

            //weapon
            _gunValue.text = $"{_weaponController.WeaponSettings[0].Ammo}";
            _blasterValue.text = $"{_weaponController.WeaponSettings[1].Ammo}";

            if (!_weaponController.BombIsReady)
            {
                _bombStatus.color = Color.white;
                _bombStatus.text = $"{_weaponController.BombTimer}";
            }
            else
            {
                _bombStatus.color = Color.green;
                _bombStatus.text = $"OK";
            }
        }
    }
}

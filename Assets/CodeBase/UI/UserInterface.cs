using CodeBase.Player;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI
{
    public class UserInterface : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private PlayerStorage playerStorage;

        [SerializeField] private Text _triesText;
        [SerializeField] public Slider _XpBar;
        [SerializeField] private TextMeshProUGUI scoreValue;
        [SerializeField] private TextMeshProUGUI lvlValue;
        [SerializeField] private Text _healthAmount;
        [SerializeField] private TextMeshProUGUI lvlProgress;
        [SerializeField] private Animator _warning;

        [Space]
        [Header("Buttons")]
        [SerializeField] private Button machineGunBttn;
        [SerializeField] private Button blasterBttn;
        [SerializeField] private Button megaBombBttn;

        [Space]
        [Header("WeaponStatus")]
        [SerializeField] private Text _gunValue;
        [SerializeField] private Text _blasterValue;
        [SerializeField] private Text _bombStatus;

        public static Action OnHealthChanged;
        public static Action OnMachineGunBttnPressed;

        public Animator Warning => _warning;

        private WeaponController _weaponController;

        private void OnEnable()
        {
            RefreshHealthInfo();

            OnHealthChanged += RefreshHealthInfo;
            //machineGunBttn.onClick.AddListener(MachineGunBttnPressed);
        }

        private void OnDisable()
        {
            OnHealthChanged -= RefreshHealthInfo;
            //machineGunBttn.onClick.RemoveListener(MachineGunBttnPressed);
        }

        private void MachineGunBttnPressed()
        {
            OnMachineGunBttnPressed?.Invoke();
        }

        private void Awake()
        {
            _weaponController = FindObjectOfType<WeaponController>();
        }

        private void RefreshHealthInfo()
        {
            //if (playerStorage.ConcretePlayer.CurrentHealth < 0)
            //    _healthAmount.text = "0";
            //else
            //    _healthAmount.text = $"{playerStorage.ConcretePlayer.CurrentHealth}";
        }

        private void Update()
        {
            //_XpBar.value = _gameSession.XP;
            //_XpBar.maxValue = _gameSession.NextLvl;

            //lvlProgress.text = $"{_XpBar.value} / {_XpBar.maxValue}";

            scoreValue.text = $"SCORE: {playerStorage.ConcretePlayer.Score}";
            //_triesText.text = $"{playerStorage.ConcretePlayer.CurrentTries}";

            lvlValue.text = $"LVL {playerStorage.ConcretePlayer.Lvl}";

            //weapon
            //_gunValue.text = $"{_weaponController.WeaponSettings[0].Ammo}";
            //_blasterValue.text = $"{_weaponController.WeaponSettings[1].Ammo}";

            //if (!_weaponController.BombIsReady)
            //{
            //    _bombStatus.color = Color.white;
            //    _bombStatus.text = $"{_weaponController.BombTimer}";
            //}
            //else
            //{
            //    _bombStatus.color = Color.green;
            //    _bombStatus.text = $"OK";
            //}
        }
    }
}

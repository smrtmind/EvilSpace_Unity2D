using CodeBase.Player;
using CodeBase.Utils;
using DG.Tweening;
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
        [SerializeField] private DependencyContainer dependencyContainer;

        //[SerializeField] private Text _triesText;
        //[SerializeField] public Slider _XpBar;
        [SerializeField] private TextMeshProUGUI healthValue;
        [SerializeField] private TextMeshProUGUI triesValue;
        [SerializeField] private TextMeshProUGUI scoreValue;
        [SerializeField] private TextMeshProUGUI lvlValue;
        [SerializeField] private Image loadingFiller;
        [SerializeField] private Button startBttn;
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private float loadingDelay;

        //[SerializeField] private Text _healthAmount;
        //[SerializeField] private TextMeshProUGUI lvlProgress;
        //[SerializeField] private Animator _warning;

        //[Space]
        //[Header("Buttons")]
        //[SerializeField] private Button machineGunBttn;
        //[SerializeField] private Button blasterBttn;
        //[SerializeField] private Button megaBombBttn;

        //[Space]
        //[Header("WeaponStatus")]
        //[SerializeField] private Text _gunValue;
        //[SerializeField] private Text _blasterValue;
        //[SerializeField] private Text _bombStatus;

        public static Action OnHealthChanged;
        public static Action OnTriesChanged;
        public static Action OnLevelLoaded;
        //public static Action OnMachineGunBttnPressed;

        //public Animator Warning => _warning;

        //private WeaponController _weaponController;

        private Tween loadingTween;

        private void OnEnable()
        {
            RefreshTriesInfo();
            RefreshHealthInfo();

            OnHealthChanged += RefreshHealthInfo;
            OnTriesChanged += RefreshTriesInfo;
            startBttn.onClick.AddListener(StartButtonPressed);

            startScreen.SetActive(true);
        }

        private void OnDisable()
        {
            OnHealthChanged -= RefreshHealthInfo;
            OnTriesChanged -= RefreshTriesInfo;
            startBttn.onClick.RemoveListener(StartButtonPressed);
        }

        private void StartButtonPressed()
        {
            startScreen.SetActive(false);
            Loading();
        }

        private void Loading()
        {
            loadingScreen.SetActive(true);
            loadingFiller.fillAmount = 0f;

            loadingTween?.Kill();
            loadingTween = loadingFiller.DOFillAmount(1f, loadingDelay <= 1f ? 1f : loadingDelay).OnComplete(() => StartLevel());

            void StartLevel()
            {
                loadingScreen.SetActive(false);
                dependencyContainer.TouchController.enabled = true;
                OnLevelLoaded?.Invoke();
            }
        }

        //private void MachineGunBttnPressed()
        //{
        //    OnMachineGunBttnPressed?.Invoke();
        //}

        //private void Awake()
        //{
        //    _weaponController = FindObjectOfType<WeaponController>();
        //}

        private void RefreshHealthInfo() => healthValue.text = $"{Mathf.Round(playerStorage.ConcretePlayer.CurrentHealth)}";

        private void RefreshTriesInfo() => triesValue.text = $"{Mathf.Round(playerStorage.ConcretePlayer.CurrentTries)}";


        private void Update()
        {
            //_XpBar.value = _gameSession.XP;
            //_XpBar.maxValue = _gameSession.NextLvl;

            //lvlProgress.text = $"{_XpBar.value} / {_XpBar.maxValue}";

            //scoreValue.text = $"SCORE: {playerStorage.ConcretePlayer.Score}";
            ////_triesText.text = $"{playerStorage.ConcretePlayer.CurrentTries}";

            //lvlValue.text = $"LVL {playerStorage.ConcretePlayer.Lvl}";

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

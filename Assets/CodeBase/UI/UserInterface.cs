using CodeBase.Player;
using CodeBase.Utils;
using DG.Tweening;
using System;
using System.Collections;
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

        [Space]
        //[SerializeField] private Text _triesText;
        //[SerializeField] public Slider _XpBar;
        [SerializeField] private TextMeshProUGUI healthValue;
        [SerializeField] private TextMeshProUGUI triesValue;
        [SerializeField] private TextMeshProUGUI scoreValue;
        [SerializeField] private TextMeshProUGUI lvlValue;
        [SerializeField] private Image loadingFiller;
        [SerializeField] private Button startBttn;
        [SerializeField] private Button exitBttn;
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private GameObject confirmationScreen;
        [SerializeField] private GameObject confirmationWindow;
        [SerializeField] private Button yesBttn;
        [SerializeField] private Button noBttn;
        [SerializeField] private float loadingDelay;
        [SerializeField] private TextMeshProUGUI loadingText;

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
        private Coroutine loadingTextCoroutine;
        private Tween loadingTextTween;
        private Tween confirmationTween;

        private void OnEnable()
        {
            RefreshTriesInfo();
            RefreshHealthInfo();

            OnHealthChanged += RefreshHealthInfo;
            OnTriesChanged += RefreshTriesInfo;
            startBttn.onClick.AddListener(StartButtonPressed);
            exitBttn.onClick.AddListener(ExitButtonPressed);
            yesBttn.onClick.AddListener(YesButtonPressed);
            noBttn.onClick.AddListener(NoButtonPressed);

            startScreen.SetActive(true);
        }

        private void OnDisable()
        {
            OnHealthChanged -= RefreshHealthInfo;
            OnTriesChanged -= RefreshTriesInfo;
            startBttn.onClick.RemoveListener(StartButtonPressed);
            exitBttn.onClick.RemoveListener(ExitButtonPressed);
            yesBttn.onClick.RemoveListener(YesButtonPressed);
            noBttn.onClick.RemoveListener(NoButtonPressed);
        }

        private void StartButtonPressed()
        {
            startScreen.SetActive(false);
            Loading();
        }

        private void ExitButtonPressed()
        {
            confirmationScreen.SetActive(true);
            confirmationWindow.transform.localScale = Vector3.zero;

            confirmationTween?.Kill();
            confirmationTween = confirmationWindow.transform.DOScale(new Vector3(1.5f, 1.5f, 1.5f), 0.25f).SetEase(Ease.Linear)
                                                            .OnComplete(() => confirmationWindow.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.Linear));
        }

        private void YesButtonPressed() => Application.Quit();

        private void NoButtonPressed()
        {
            confirmationTween?.Kill();
            confirmationTween = confirmationWindow.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear)
                                                            .OnComplete(() => confirmationScreen.SetActive(false));
        }

        private void Loading()
        {
            loadingScreen.SetActive(true);
            loadingFiller.fillAmount = 0f;
            loadingTextCoroutine = StartCoroutine(ShakeLoadingText());

            loadingTween?.Kill();
            loadingTween = loadingFiller.DOFillAmount(1f, loadingDelay <= 1f ? 1f : loadingDelay).OnComplete(() => StartLevel());

            void StartLevel()
            {
                loadingScreen.SetActive(false);
                dependencyContainer.TouchController.enabled = true;
                StopCoroutine(loadingTextCoroutine);
                OnLevelLoaded?.Invoke();
            }
        }

        private IEnumerator ShakeLoadingText()
        {
            while (true)
            {
                loadingTextTween?.Kill();
                loadingTextTween = loadingText.transform.DOPunchScale(new Vector2(0.1f, 0.1f), 0.25f);

                yield return new WaitForSeconds(0.5f);
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

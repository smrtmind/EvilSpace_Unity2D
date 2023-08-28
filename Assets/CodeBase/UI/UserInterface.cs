using CodeBase.Player;
using CodeBase.Utils;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CodeBase.UI
{
    public class UserInterface : MonoBehaviour
    {
        #region Variables
        [Header("Storages")]
        [SerializeField] private PlayerStorage playerStorage;

        [Header("UI")]
        [SerializeField] private TextMeshProUGUI healthValue;
        [SerializeField] private TextMeshProUGUI triesValue;
        [SerializeField] private TextMeshProUGUI lvlValue;
        [SerializeField] private TextMeshProUGUI scoreValue;
        [SerializeField] private Vector3 scoreValueScaleOnChange;
        [SerializeField] private Slider levelProgressSlider;

        [Header("Buttons")]
        [SerializeField] private Button bombBttn;
        [SerializeField] private Image bombReloadFiller;
        [SerializeField] private TextMeshProUGUI bombTimerValue;

        [Space]
        [SerializeField] private float delayToAutoHidePanel = 10f;
        [SerializeField] private GameObject optionsPanel;
        [SerializeField] private Button optionsBttn;
        [SerializeField] private Button soundBttn;
        [SerializeField] private Sprite soundOn;
        [SerializeField] private Sprite soundOff;
        [SerializeField] private Button vibroBttn;
        [SerializeField] private Sprite vibroOn;
        [SerializeField] private Sprite vibroOff;

        [Header("Loading Screen")]
        [SerializeField] private GameObject loadingScreen;
        [SerializeField] private Image loadingFiller;
        [SerializeField] private float loadingDelay;
        [SerializeField] private TextMeshProUGUI loadingText;

        [Header("Game Over Screen")]
        [SerializeField] private GameObject gameOverScreen;
        [SerializeField] private TextMeshProUGUI finalScoreValue;
        [SerializeField] private Button replayBttn;
        [SerializeField] private Button exitGameBttn;

        [Header("Start Screen")]
        [SerializeField] private GameObject startScreen;
        [SerializeField] private GameObject confirmationScreen;
        [SerializeField] private GameObject confirmationWindow;
        [SerializeField] private Button startBttn;
        [SerializeField] private Button exitBttn;
        [SerializeField] private Button yesBttn;
        [SerializeField] private Button noBttn;

        private Tween loadingTween;
        private Coroutine loadingTextCoroutine;
        private Tween loadingTextTween;
        private Tween confirmationTween;
        private bool scoreIsScaling;
        private Tween bombFillerTween;
        private WeaponController weaponController;
        private Tween optionsPanelTween;
        private Coroutine hidePanelRoutine;
        #endregion

        [Inject]
        private void Construct(WeaponController weapon)
        {
            weaponController = weapon;
        }

        private void OnEnable()
        {
            EventObserver.OnHealthChanged += RefreshHealthInfo;
            EventObserver.OnTriesChanged += RefreshTriesInfo;
            EventObserver.OnScoreChanged += RefreshScoreInfo;
            EventObserver.OnLevelChanged += RefreshLevelInfo;
            EventObserver.OnLevelProgressChanged += RefreshLevelProgressSlider;
            EventObserver.OnGameOver += ShowGameOverScreen;
            EventObserver.OnPlayerDied += DisableBombButton;

            startBttn.onClick.AddListener(StartButtonPressed);
            exitBttn.onClick.AddListener(ExitButtonPressed);
            yesBttn.onClick.AddListener(YesButtonPressed);
            noBttn.onClick.AddListener(NoButtonPressed);
            replayBttn.onClick.AddListener(ReplayButtonPressed);
            exitGameBttn.onClick.AddListener(YesButtonPressed);
            bombBttn.onClick.AddListener(BombButtonPressed);
            optionsBttn.onClick.AddListener(ShowOptionsPanel);
            soundBttn.onClick.AddListener(EnableSound);
            vibroBttn.onClick.AddListener(EnableVibrations);
        }

        private void OnDisable()
        {
            EventObserver.OnHealthChanged -= RefreshHealthInfo;
            EventObserver.OnTriesChanged -= RefreshTriesInfo;
            EventObserver.OnScoreChanged -= RefreshScoreInfo;
            EventObserver.OnLevelChanged -= RefreshLevelInfo;
            EventObserver.OnLevelProgressChanged -= RefreshLevelProgressSlider;
            EventObserver.OnGameOver -= ShowGameOverScreen;
            EventObserver.OnPlayerDied -= DisableBombButton;

            startBttn.onClick.RemoveListener(StartButtonPressed);
            exitBttn.onClick.RemoveListener(ExitButtonPressed);
            yesBttn.onClick.RemoveListener(YesButtonPressed);
            noBttn.onClick.RemoveListener(NoButtonPressed);
            replayBttn.onClick.RemoveListener(ReplayButtonPressed);
            exitGameBttn.onClick.RemoveListener(YesButtonPressed);
            bombBttn.onClick.RemoveListener(BombButtonPressed);
            optionsBttn.onClick.RemoveListener(ShowOptionsPanel);
            soundBttn.onClick.RemoveListener(EnableSound);
            vibroBttn.onClick.RemoveListener(EnableVibrations);
        }

        private void Start()
        {
            startScreen.SetActive(true);

            bombReloadFiller.fillAmount = 0f;
            bombTimerValue.gameObject.SetActive(false);
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

        private void ReplayButtonPressed()
        {
            EventObserver.OnGameRestarted?.Invoke();

            gameOverScreen.SetActive(false);
            Time.timeScale = 1f;
            Loading();
        }

        private void YesButtonPressed() => Application.Quit();

        private void NoButtonPressed()
        {
            confirmationTween?.Kill();
            confirmationTween = confirmationWindow.transform.DOScale(Vector3.zero, 0.25f).SetEase(Ease.Linear)
                                                            .OnComplete(() => confirmationScreen.SetActive(false));
        }

        private void BombButtonPressed()
        {
            bombReloadFiller.fillAmount = 1f;
            bombBttn.interactable = false;

            bombFillerTween?.Kill();
            bombFillerTween = bombReloadFiller.DOFillAmount(0f, weaponController.BombReloadDelay).SetEase(Ease.Linear)
                .OnComplete(() => bombBttn.interactable = true);

            StartCoroutine(StartBombTimer());

            EventObserver.OnBombButtonPressed?.Invoke();
        }

        private IEnumerator StartBombTimer()
        {
            bombTimerValue.gameObject.SetActive(true);

            var timer = weaponController.BombReloadDelay;
            while (timer > 0f)
            {
                bombTimerValue.text = $"{Mathf.Floor(timer)}";
                yield return timer -= Time.deltaTime;
            }

            bombTimerValue.gameObject.SetActive(false);
        }

        private void DisableBombButton()
        {
            bombBttn.interactable = false;
            Invoke(nameof(EnableBombButton), 3.5f);
        }

        private void EnableBombButton() => bombBttn.interactable = true;

        private void Loading()
        {
            loadingScreen.SetActive(true);
            loadingFiller.fillAmount = 0f;
            bombReloadFiller.fillAmount = 0f;
            loadingTextCoroutine = StartCoroutine(ShakeLoadingText());

            loadingTween?.Kill();
            loadingTween = loadingFiller.DOFillAmount(1f, loadingDelay <= 1f ? 1f : loadingDelay).OnComplete(() => StartLevel());

            void StartLevel()
            {
                RefreshHealthInfo();
                RefreshTriesInfo();
                RefreshScoreInfo();
                RefreshLevelInfo();
                RefreshLevelProgressSlider();
                RefreshOptionsPanel();

                loadingScreen.SetActive(false);
                StopCoroutine(loadingTextCoroutine);
                EventObserver.OnLevelLoaded?.Invoke();
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

        private void ShowGameOverScreen()
        {
            gameOverScreen.SetActive(true);
            Time.timeScale = 0f;
            finalScoreValue.text = $"final score: {Mathf.Round(playerStorage.PlayerData.Score)}";
        }

        private void ShowOptionsPanel()
        {
            if (optionsPanel.activeSelf)
            {
                if (hidePanelRoutine != null)
                {
                    StopCoroutine(hidePanelRoutine);
                    hidePanelRoutine = StartCoroutine(HideOptionsPanel());
                }
            }
            else
            {
                optionsPanel.transform.localScale = Vector3.zero;
                optionsPanel.SetActive(true);

                optionsPanelTween?.Kill();
                optionsPanelTween = optionsPanel.transform.DOScale(Vector3.one, 0.15f).SetEase(Ease.Linear);

                if (hidePanelRoutine == null)
                    hidePanelRoutine = StartCoroutine(HideOptionsPanel(delayToAutoHidePanel));
            }
        }

        private IEnumerator HideOptionsPanel(float delay = 0f)
        {
            yield return new WaitForSeconds(delay);

            optionsPanelTween?.Kill();
            optionsPanelTween = optionsPanel.transform.DOScale(Vector3.zero, 0.15f).SetEase(Ease.Linear)
                .OnComplete(() => optionsPanel.SetActive(false));

            hidePanelRoutine = null;
        }

        private void EnableSound()
        {
            playerStorage.PlayerData.EnableSound(!playerStorage.PlayerData.SoundOn);
            RefreshOptionsPanel();

            StopCoroutine(hidePanelRoutine);
            hidePanelRoutine = StartCoroutine(HideOptionsPanel(delayToAutoHidePanel));

            EventObserver.OnSoundActivated?.Invoke(playerStorage.PlayerData.SoundOn);
        }

        private void EnableVibrations()
        {
            playerStorage.PlayerData.EnableVibrations(!playerStorage.PlayerData.VibrationsOn);
            RefreshOptionsPanel();

            StopCoroutine(hidePanelRoutine);
            hidePanelRoutine = StartCoroutine(HideOptionsPanel(delayToAutoHidePanel));

            EventObserver.OnVibrationsActivated?.Invoke(playerStorage.PlayerData.VibrationsOn);
        }

        private void RefreshOptionsPanel()
        {
            soundBttn.image.sprite = playerStorage.PlayerData.SoundOn ? soundOn : soundOff;
            vibroBttn.image.sprite = playerStorage.PlayerData.VibrationsOn ? vibroOn : vibroOff;
        }

        private void RefreshHealthInfo() => healthValue.text = $"{Mathf.Round(playerStorage.PlayerData.CurrentHealth)}";

        private void RefreshTriesInfo() => triesValue.text = $"{playerStorage.PlayerData.CurrentTries}";

        private void RefreshLevelInfo() => lvlValue.text = $"lvl {playerStorage.PlayerData.Lvl}";

        private void RefreshLevelProgressSlider()
        {
            levelProgressSlider.minValue = 0f;
            levelProgressSlider.maxValue = playerStorage.PlayerData.TargetLvlProgress;
            levelProgressSlider.value = playerStorage.PlayerData.CurrentLevelProgress;
        }

        private void RefreshScoreInfo()
        {
            scoreValue.text = $"score {Mathf.Round(playerStorage.PlayerData.Score)}";

            if (!scoreIsScaling)
            {
                scoreIsScaling = true;
                scoreValue.transform.DOPunchScale(scoreValueScaleOnChange, 0.25f)
                                    .OnComplete(() => scoreIsScaling = false);
            }
        }
    }
}

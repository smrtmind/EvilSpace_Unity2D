using CodeBase.Player;
using CodeBase.Utils;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

        [Space]
        [SerializeField] private TextMeshProUGUI scoreValue;
        [SerializeField] private Vector3 scoreValueScaleOnChange;

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
        #endregion

        private void OnEnable()
        {
            EventObserver.OnHealthChanged += RefreshHealthInfo;
            EventObserver.OnTriesChanged += RefreshTriesInfo;
            EventObserver.OnScoreChanged += RefreshScoreInfo;
            EventObserver.OnGameOver += ShowGameOverScreen;

            startBttn.onClick.AddListener(StartButtonPressed);
            exitBttn.onClick.AddListener(ExitButtonPressed);
            yesBttn.onClick.AddListener(YesButtonPressed);
            noBttn.onClick.AddListener(NoButtonPressed);
            replayBttn.onClick.AddListener(ReplayButtonPressed);
            exitGameBttn.onClick.AddListener(YesButtonPressed);

            startScreen.SetActive(true);
        }

        private void OnDisable()
        {
            EventObserver.OnHealthChanged -= RefreshHealthInfo;
            EventObserver.OnTriesChanged -= RefreshTriesInfo;
            EventObserver.OnScoreChanged -= RefreshScoreInfo;
            EventObserver.OnGameOver -= ShowGameOverScreen;

            startBttn.onClick.RemoveListener(StartButtonPressed);
            exitBttn.onClick.RemoveListener(ExitButtonPressed);
            yesBttn.onClick.RemoveListener(YesButtonPressed);
            noBttn.onClick.RemoveListener(NoButtonPressed);
            replayBttn.onClick.RemoveListener(ReplayButtonPressed);
            exitGameBttn.onClick.RemoveListener(YesButtonPressed);
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

        private void Loading()
        {
            loadingScreen.SetActive(true);
            loadingFiller.fillAmount = 0f;
            loadingTextCoroutine = StartCoroutine(ShakeLoadingText());

            loadingTween?.Kill();
            loadingTween = loadingFiller.DOFillAmount(1f, loadingDelay <= 1f ? 1f : loadingDelay).OnComplete(() => StartLevel());

            void StartLevel()
            {
                RefreshHealthInfo();
                RefreshTriesInfo();
                RefreshScoreInfo();

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
            finalScoreValue.text = $"final score: {Mathf.Round(playerStorage.ConcretePlayer.Score)}";
        }

        private void RefreshHealthInfo() => healthValue.text = $"{Mathf.Round(playerStorage.ConcretePlayer.CurrentHealth)}";

        private void RefreshTriesInfo() => triesValue.text = $"{playerStorage.ConcretePlayer.CurrentTries}";

        private void RefreshScoreInfo()
        {
            scoreValue.text = $"{Mathf.Round(playerStorage.ConcretePlayer.Score)}";

            if (!scoreIsScaling)
            {
                scoreIsScaling = true;
                scoreValue.transform.DOPunchScale(scoreValueScaleOnChange, 0.25f)
                                    .OnComplete(() => scoreIsScaling = false);
            }
        }
    }
}

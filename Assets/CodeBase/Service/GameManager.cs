using CodeBase.Player;
using System;
using UnityEngine;

namespace CodeBase.Service
{
    public class GameManager : MonoBehaviour
    {
        [Header("Storages")][SerializeField] private PlayerStorage playerStorageSO = default;
        //[SerializeField] private DependencyContainer dependencyContainerSO = default;

        [field: Header("Debug Mode")]
        [field: SerializeField]
        public static bool IsDevMode { get; private set; } = false;

        [SerializeField] private bool isDevMode = false;

        [Header("Global Actions")] public static Action GameStartAction = default;
        public static Action PlayerLoadedAction = default;
        public static Action LevelPrepareAction = default;
        public static Action LevelReadyAction = default;
        public static Action LevelStartAction = default;
        public static Action StartCollectPeriodAction = default;
        public static Action RestartCollectPeriodAction = default;
        public static Action StartWarPeriodAction = default;
        //public static Action<LevelResult> LevelFinishAction = default;

        [Header("Variables")] private bool playerIsLoaded = false;
        //private float startLevelTime = default;


        private void Awake()
        {
            //playerStorageSO.LoadPlayer();

            IsDevMode = isDevMode;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void OnEnable()
        {
            playerStorageSO.LoadPlayer();
            //LevelStartAction += StartLevel;
            LevelReadyAction += LevelReady;
            //LevelFinishAction += LevelFinish;
            PlayerLoadedAction += PlayerLoaded;
            //GameStartAction += StartGame;
        }

        //private void Awake()
        //{
        //    //playerStorageSO.LoadPlayer();
        //    GameStartAction?.Invoke();
        //}

        private void OnDisable()
        {
            //LevelStartAction -= StartLevel;
            PlayerLoadedAction -= PlayerLoaded;
            LevelReadyAction -= LevelReady;
            //LevelFinishAction -= LevelFinish;
            //GameStartAction -= StartGame;
        }

        private void OnDestroy()
        {
            if (playerIsLoaded)
            {
                playerStorageSO.SavePlayer();
            }
        }

        private void OnApplicationQuit()
        {
            if (playerIsLoaded)
            {
                playerStorageSO.SavePlayer();
            }
        }

        private void OnApplicationPause(bool _pause)
        {
            if (_pause && playerIsLoaded)
            {
                playerStorageSO.SavePlayer();
            }
        }

        private void SavePlayer()
        {
            if (playerIsLoaded)
            {
                playerStorageSO.SavePlayer();
            }
        }

        private void LevelReady()
        {
            LevelStartAction?.Invoke();
        }

        private void PlayerLoaded()
        {
            playerIsLoaded = true;
            LevelPrepareAction?.Invoke();
        }
    }
}

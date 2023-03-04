using CodeBase.Player;
using System;
using UnityEngine;

namespace CodeBase.Service
{
    public class GameManager : MonoBehaviour
    {
        [Header("Storages")][SerializeField] private PlayerStorage playerStorageSO = default;

        [field: Header("Debug Mode")]
        [field: SerializeField]

        [Header("Global Actions")] public static Action GameStartAction = default;
        public static Action PlayerLoadedAction = default;
        public static Action LevelPrepareAction = default;
        public static Action LevelReadyAction = default;
        public static Action LevelStartAction = default;
        public static Action StartCollectPeriodAction = default;
        public static Action RestartCollectPeriodAction = default;
        public static Action StartWarPeriodAction = default;

        [Header("Variables")] private bool playerIsLoaded = false;


        private void Awake()
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        private void OnEnable()
        {
            playerStorageSO.LoadPlayer();
            LevelReadyAction += LevelReady;
            PlayerLoadedAction += PlayerLoaded;
        }

        private void OnDisable()
        {
            PlayerLoadedAction -= PlayerLoaded;
            LevelReadyAction -= LevelReady;
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

using System;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Utils
{
    public class EventObserver
    {
        [Header("Player events")]
        public static Action OnPlayerHit;
        public static Action OnPlayerDied;
        public static Action<Vector3> OnPlayerCollision;

        [Header("Camera events")]
        public static Action<float, float> OnShakeCamera;

        [Header("UI events")]
        public static Action OnHealthChanged;
        public static Action OnTriesChanged;
        public static Action OnLevelChanged;
        public static Action OnScoreChanged;
        public static Action OnLevelLoaded;
        public static Action OnLevelProgressChanged;
        public static Action OnGameRestarted;
        public static Action OnGameOver;
        public static Action OnBombButtonPressed;
        public static Action<CollectableType, Color> OnCollectableGot;

        [Header("Touch events")]
        public static Action<bool> OnStartMoving;

        [Header("Settings events")]
        public static Action<bool> OnSoundActivated;
        public static Action<bool> OnVibrationsActivated;
    }
}

using CodeBase.Utils;
using System;
using UnityEngine;

namespace CodeBase.Player
{
    [Serializable]
    public class Player
    {
        [field: Header("Device Settings")]
        [field: SerializeField] public bool SoundOn { get; private set; } = true;
        [field: SerializeField] public bool VibrationsOn { get; private set; } = true;

        [field: Header("Player Settings")]
        [field: SerializeField] public bool IsDead { get; private set; } = false;

        [field: Space]
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float CurrentHealth { get; private set; }

        [field: Space]
        [field: SerializeField] public int Tries { get; private set; }
        [field: SerializeField] public int CurrentTries { get; private set; }

        [field: Space]
        [field: SerializeField] public int Lvl { get; private set; } = 1;
        [field: SerializeField] public float CurrentLevelProgress { get; private set; } = 0f;
        [field: SerializeField] public float TargetLvlProgress { get; private set; }
        [field: SerializeField] public float DefaultTargetLvlProgress { get; private set; }
        [field: SerializeField] public float AdditionalPercentPerLevel { get; private set; }

        [field: Space]
        [field: SerializeField] public float Score { get; private set; } = 0f;
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public Vector3 DefaultPlayerPosition { get; private set; }

        [field: Header("Default Values")]
        [field: SerializeField] public float DefaultHealth { get; private set; }
        [field: SerializeField] public int DefaultTries { get; private set; }
        [field: SerializeField] public float DefaultMovementSpeed { get; private set; }

        private const int MAX_LEVEL = 99;
        private const int LEVEL_TO_EVOLVE = 5;
        private const float MAX_HEALTH = 10f;

        public void SetPlayerData(float health, int tries, float defaultLevelProgressTarget, float additionalPercentPerLevel, float movementSpeed, Vector3 defaultPlayerPosition)
        {
            Health = health;
            Tries = tries;
            TargetLvlProgress = defaultLevelProgressTarget;
            DefaultTargetLvlProgress = defaultLevelProgressTarget;
            AdditionalPercentPerLevel = additionalPercentPerLevel;
            MovementSpeed = movementSpeed;
            DefaultPlayerPosition = defaultPlayerPosition;

            CurrentHealth = Health;
            DefaultHealth = Health;

            CurrentTries = Tries;
            DefaultTries = Tries;

            DefaultMovementSpeed = MovementSpeed;
        }

        public void ModifyHealth(float health)
        {
            CurrentHealth += health;
            if (CurrentHealth <= 0f)
            {
                IsDead = true;
                CurrentHealth = 0f;
            }

            EventObserver.OnHealthChanged?.Invoke();
        }

        public void ModifyTries(int tries)
        {
            CurrentTries += tries;
            if (CurrentTries <= 0)
                CurrentTries = 0;

            EventObserver.OnTriesChanged?.Invoke();
        }

        public void ModifyScore(float score)
        {
            Score += score;
            EventObserver.OnScoreChanged?.Invoke();
        }

        public void ModifyLevelProgress(float exp)
        {
            CurrentLevelProgress += exp;

            if (CurrentLevelProgress >= TargetLvlProgress)
            {
                var result = CurrentLevelProgress - TargetLvlProgress;
                CurrentLevelProgress = result;

                var percent = (TargetLvlProgress / 100f) * AdditionalPercentPerLevel;
                TargetLvlProgress += percent;

                IncreaseLevel();
            }

            EventObserver.OnLevelProgressChanged?.Invoke();
        }

        private void IncreaseLevel()
        {
            if (Lvl < MAX_LEVEL) Lvl++;

            if (Lvl % LEVEL_TO_EVOLVE == 0 && Health < MAX_HEALTH) Health++;
            CurrentHealth = Health;

            EventObserver.OnHealthChanged?.Invoke();
            EventObserver.OnLevelChanged?.Invoke();
        }

        public void RevivePlayer()
        {
            ModifyHealth(Health);
            ModifyTries(-1);
            IsDead = false;
        }

        public void StartNewGame()
        {
            IsDead = false;
            ModifyHealth(DefaultHealth);
            ModifyTries(DefaultTries);
            ModifyScore(-Score);
            MovementSpeed = DefaultMovementSpeed;
            CurrentLevelProgress = 0f;
            TargetLvlProgress = DefaultTargetLvlProgress;
            Lvl = 1;
        }

        public void EnableSound(bool enable) => SoundOn = enable;

        public void EnableVibrations(bool enable) => VibrationsOn = enable;
    }
}

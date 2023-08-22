using CodeBase.Utils;
using System;
using UnityEngine;

namespace CodeBase.Player
{
    [Serializable]
    public class Player
    {
        [field: Header("Player Settings")]
        [field: SerializeField] public bool IsDead { get; private set; }

        [field: Space]
        [field: SerializeField] public float Health { get; private set; }
        [field: SerializeField] public float CurrentHealth { get; private set; }

        [field: Space]
        [field: SerializeField] public int Tries { get; private set; }
        [field: SerializeField] public int CurrentTries { get; private set; }

        [field: Space]
        [field: SerializeField] public int Lvl { get; private set; }
        [field: SerializeField] public float Score { get; private set; }
        [field: SerializeField] public float MovementSpeed { get; private set; }
        [field: SerializeField] public Vector3 DefaultPlayerPosition { get; private set; }

        [field: Header("Default Values")]
        [field: SerializeField] public float DefaultHealth { get; private set; }
        [field: SerializeField] public int DefaultTries { get; private set; }
        [field: SerializeField] public float DefaultMovementSpeed { get; private set; }

        public void SetPlayerData(float health, int tries, int lvl, float score, float movementSpeed, Vector3 defaultPlayerPosition)
        {
            IsDead = false;
            Health = health;
            Tries = tries;
            Lvl = lvl;
            Score = score;
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

        private void SetPlayerLvl(int lvl)
        {
            Lvl = lvl;
            EventObserver.OnLevelChanged?.Invoke();
        }

        public void ModifyScore(float score)
        {
            Score += score;
            EventObserver.OnScoreChanged?.Invoke();
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
            SetPlayerLvl(1);
            ModifyScore(-Score);
            MovementSpeed = DefaultMovementSpeed;
        }
    }
}

using CodeBase.UI;
using System;
using UnityEngine;

namespace CodeBase.Player
{
    [Serializable]
    public class Player
    {
        [field: Header("Player Progress")]
        [field: SerializeField] public float Score { get; private set; }
        [field: SerializeField] public bool IsDead { get; private set; }

        [field: Header("Player Settings")]
        [field: SerializeField] public Vector3 DefaultPlayerPosition { get; private set; }
        [field: SerializeField] public int Lvl { get; private set; }

        [field: Space]
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float CurrentHealth { get; private set; }

        [field: Space]
        [field: SerializeField] public int Tries { get; private set; }
        [field: SerializeField] public int CurrentTries { get; private set; }

        [field: Space]
        [field: SerializeField] public float MovementSpeed { get; private set; }

        public bool GameIsOver => CurrentTries == 0;

        public void SetPlayerData(Vector3 defaultPlayerPosition, int lvl, float score, float health, int tries, float movementSpeed)
        {
            IsDead = false;
            DefaultPlayerPosition = defaultPlayerPosition;
            Lvl = lvl;
            Score = score;
            MaxHealth = health;
            Tries = tries;
            MovementSpeed = movementSpeed;

            CurrentHealth = MaxHealth;
            CurrentTries = Tries;
        }

        public void ModifyHealth(float health)
        {
            CurrentHealth += health;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                CurrentTries--;
                PlayerIsDead(true);
            }

            PlayerController.OnPlayerDamaged?.Invoke();
            UserInterface.OnHealthChanged?.Invoke();
        }

        public void PlayerIsDead(bool status) => IsDead = status;
    }
}

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
        [field: SerializeField] public bool GameIsOver { get; private set; }

        [field: Header("Player Settings")]
        [field: SerializeField] public Vector3 PlayerDefaultPosition { get; private set; }

        [field: Space]
        [field: SerializeField] public float MaxHealth { get; private set; }
        [field: SerializeField] public float CurrentHealth { get; private set; }

        [field: Space]
        [field: SerializeField] public int Tries { get; private set; }
        [field: SerializeField] public int CurrentTries { get; private set; }

        [field: Space]
        [field: SerializeField] public float MovementSpeed { get; private set; }
        //[field: SerializeField] public List<Technology> FactoryParameters { get; private set; } = new List<Technology>();

        //public void SetPlayerData(int caseCapacity, float mainConveyorSpeedDividor, double money, double bank, bool tutorialCompleted)
        //{
        //    CaseCapacity = caseCapacity;
        //    MainConveyorSpeedDividor = mainConveyorSpeedDividor;
        //    Money = money;
        //    Bank = bank;
        //    TutorialCompleted = tutorialCompleted;
        //}

        //public Technology GetFactoryTechData(CrystalType type)
        //{
        //    foreach (var parameter in FactoryParameters)
        //        if (type == parameter.FactoryType)
        //            return parameter;

        //    return null;
        //}


        public void ModifyHealth(float health)
        {
            CurrentHealth += health;
            if (CurrentHealth <= 0)
            {
                CurrentHealth = 0;
                CurrentTries--;
                IsDead = true;

                if (CurrentTries == 0)
                {
                    GameIsOver = true;
                }
            }
        }

        public void ResetAllPlayerData()
        {
            Score = 0f;
            CurrentHealth = MaxHealth;
            CurrentTries = Tries;
            IsDead = false;
            GameIsOver = false;
        }
    }
}

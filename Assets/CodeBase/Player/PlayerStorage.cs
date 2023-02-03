using UnityEngine;

namespace CodeBase.Player
{
    [CreateAssetMenu(fileName = "PlayerStorage", menuName = "ScriptableObjects/PlayerStorage")]
    public class PlayerStorage : ScriptableObject
    {
        [SerializeField] private string playerPrefsSaveString = "_playerSave";
        [SerializeField] private PlayerDataStorage playerDataStorage;

        [Header("ConcretePlayer")]
        [SerializeField] private Player concretePlayer = new Player();

        public Player ConcretePlayer => concretePlayer;

        public float MovementSpeed { get; internal set; }

        public void SavePlayer()
        {
            Debug.Log("SAVED");
            PlayerPrefs.SetString(playerPrefsSaveString, JsonUtility.ToJson(concretePlayer));
        }

        public void LoadPlayer()
        {
            var playerString = PlayerPrefs.GetString(playerPrefsSaveString, "");
            if (playerString != "")
            {
                Debug.Log("LOADED");
                concretePlayer = JsonUtility.FromJson<Player>(playerString);
            }
            else
            {
                Debug.Log("NEW GAME");
                concretePlayer = new Player();
                InitPlayer();
            }
        }

        private void InitPlayer()
        {
            concretePlayer.ResetAllPlayerData();
            //CrystalType[] factoryTypes = { CrystalType.Red, CrystalType.Blue, CrystalType.Pink, CrystalType.Green, CrystalType.Violet };
            //for (int i = 0; i < factoryTypes.Length; i++)
            //{
            //    var tech = technologyStorage.GetTechData(factoryTypes[i]);

            //    Technology newTech = new Technology(tech.FactoryType,
            //                                        tech.FactoryCost,
            //                                        tech.Level,
            //                                        tech.SpawnDelay,
            //                                        tech.BaseProfit,
            //                                        tech.BaseUpgradeCost,
            //                                        tech.UpgradePowerCoeficient);

            //    concretePlayer.FactoryParameters.Add(newTech);
            //}

            //concretePlayer.SetPlayerData(technologyStorage.CaseCapacity,
            //                             technologyStorage.MainConveyorSpeedDividor,
            //                             technologyStorage.Money,
            //                             technologyStorage.Bank,
            //                             technologyStorage.TutorialCompleted);
        }
    }
}

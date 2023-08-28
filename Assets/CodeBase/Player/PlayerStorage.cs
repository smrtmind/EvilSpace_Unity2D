using UnityEngine;

namespace CodeBase.Player
{
    [CreateAssetMenu(fileName = "PlayerStorage", menuName = "ScriptableObjects/PlayerStorage")]
    public class PlayerStorage : ScriptableObject
    {
        [SerializeField] private string playerPrefsSaveString = "_playerSave";
        [SerializeField] private PlayerDataStorage playerDataStorage;

        [Header("ConcretePlayer")]
        [SerializeField] private Player playerData = new Player();

        public Player PlayerData => playerData;

        public void SavePlayer()
        {
            Debug.Log("SAVED");
            PlayerPrefs.SetString(playerPrefsSaveString, JsonUtility.ToJson(playerData));
        }

        public void LoadPlayer()
        {
            var playerString = PlayerPrefs.GetString(playerPrefsSaveString, "");
            if (playerString != "")
            {
                Debug.Log("LOADED");
                playerData = JsonUtility.FromJson<Player>(playerString);
            }
            else
            {
                Debug.Log("NEW GAME");
                playerData = new Player();
                InitPlayer();
            }
        }

        private void InitPlayer()
        {
            playerData.SetPlayerData(playerDataStorage.DefaultHealth,
                                     playerDataStorage.DefaultTries,
                                     playerDataStorage.DefaultLevelProgressTarget,
                                     playerDataStorage.AdditionalPercentPerLevel,
                                     playerDataStorage.DefaultMovementSpeed,
                                     playerDataStorage.DefaultPlayerPosition);
        }
    }
}

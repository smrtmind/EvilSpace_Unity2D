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
            concretePlayer.SetPlayerData(playerDataStorage.DefaultPlayerPosition,
                                         playerDataStorage.DefaultLvl,
                                         playerDataStorage.DefaulScore,
                                         playerDataStorage.DefaulHealth,
                                         playerDataStorage.DefaultTries,
                                         playerDataStorage.DefaultMovementSpeed);
        }
    }
}

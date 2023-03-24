using UnityEngine;

namespace CodeBase.Player
{
    [CreateAssetMenu(fileName = "PlayerDataStorage", menuName = "ScriptableObjects/PlayerDataStorage")]
    public class PlayerDataStorage : ScriptableObject
    {
        [field: Header("Player Settings")]
        [field: SerializeField] public float DefaultHealth { get; private set; }
        [field: SerializeField] public int DefaultTries { get; private set; }
        [field: SerializeField] public int DefaultLvl { get; private set; }
        [field: SerializeField] public float DefaultScore { get; private set; }
        [field: SerializeField] public float DefaultMovementSpeed { get; private set; }
        [field: SerializeField] public Vector3 DefaultPlayerPosition { get; private set; }
    }
}

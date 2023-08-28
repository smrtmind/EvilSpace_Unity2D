using UnityEngine;

namespace CodeBase.Player
{
    [CreateAssetMenu(fileName = "PlayerDataStorage", menuName = "ScriptableObjects/PlayerDataStorage")]
    public class PlayerDataStorage : ScriptableObject
    {
        [field: Header("Player Settings")]
        [field: SerializeField] public float DefaultHealth { get; private set; }
        [field: SerializeField] public int DefaultTries { get; private set; }

        [field: Space]
        [field: SerializeField] public float DefaultLevelProgressTarget { get; private set; }
        [field: SerializeField] public float AdditionalPercentPerLevel { get; private set; }

        [field: Space]
        [field: SerializeField] public float DefaultMovementSpeed { get; private set; }
        [field: SerializeField] public Vector3 DefaultPlayerPosition { get; private set; }
    }
}

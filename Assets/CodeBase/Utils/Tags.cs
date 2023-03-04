using UnityEngine;

namespace CodeBase.Utils
{
    public static class Tags
    {
        [field: SerializeField] public static string Player { get; private set; } = "Player";
        [field: SerializeField] public static string Enemy { get; private set; } = "Enemy";
    }
}

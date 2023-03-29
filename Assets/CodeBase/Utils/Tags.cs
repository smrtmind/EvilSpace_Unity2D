using UnityEngine;

namespace CodeBase.Utils
{
    public static class Tags
    {
        [field: SerializeField] public static string Player { get; private set; } = "Player";
        [field: SerializeField] public static string Enemy { get; private set; } = "Enemy";
        [field: SerializeField] public static string Projectile { get; private set; } = "Projectile";
        [field: SerializeField] public static string EnemyProjectile { get; private set; } = "EnemyProjectile";
        [field: SerializeField] public static string Shield { get; private set; } = "Shield";
    }
}

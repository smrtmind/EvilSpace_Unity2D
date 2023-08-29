using CodeBase.Mobs;
using CodeBase.ObjectBased;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Utils
{
    public static class Dictionaries
    {
        public static Dictionary<Transform, Projectile> Projectiles { get; private set; } = new Dictionary<Transform, Projectile>();
        public static Dictionary<Transform, EnemyProjectile> EnemyProjectiles { get; private set; } = new Dictionary<Transform, EnemyProjectile>();
        public static Dictionary<Transform, Enemy> Enemies { get; private set; } = new Dictionary<Transform, Enemy>();
    }
}

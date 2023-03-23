using CodeBase.ObjectBased;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Utils
{
    public static class Dictionaries
    {
        public static Dictionary<Transform, Projectile> PlayerProjectiles { get; private set; } = new Dictionary<Transform, Projectile>();
        public static Dictionary<Transform, Projectile> EnemyProjectiles { get; private set; } = new Dictionary<Transform, Projectile>();
    }
}

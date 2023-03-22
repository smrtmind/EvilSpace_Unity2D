using CodeBase.ObjectBased;
using System.Collections.Generic;
using UnityEngine;

namespace CodeBase.Utils
{
    public static class Dictionaries
    {
        public static Dictionary<Transform, Projectile> Projectiles { get; private set; } = new Dictionary<Transform, Projectile>();
    }
}

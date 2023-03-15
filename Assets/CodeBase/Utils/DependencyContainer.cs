using CodeBase.Player;
using UnityEngine;

namespace CodeBase.Utils
{
    [CreateAssetMenu(fileName = "DependencyContainer", menuName = "ScriptableObjects/DependencyContainer")]
    public class DependencyContainer : ScriptableObject
    {
        public Pool Pool { get; set; }
        public Camera MainCamera { get; set; }
        public ScreenBounds ScreenBounds { get; set; }
        public TouchController TouchController { get; set; }
    }
}

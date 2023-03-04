using UnityEngine;

namespace CodeBase.Utils
{
    [CreateAssetMenu(fileName = "DependencyContainer", menuName = "ScriptableObjects/DependencyContainer")]
    public class DependencyContainer : ScriptableObject
    {
        public Pool Pool { get; set; }
        public Camera MainCamera { get; set; }
    }
}

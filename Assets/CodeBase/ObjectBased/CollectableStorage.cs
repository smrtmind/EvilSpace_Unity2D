using System.Linq;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.ObjectBased
{
    [CreateAssetMenu(fileName = "CollectableStorage", menuName = "ScriptableObjects/CollectableStorage")]
    public class CollectableStorage : ScriptableObject
    {
        [SerializeField] private Collectable[] collectables;

        public Collectable GetCollectableInfo(CollectableType type) => collectables.FirstOrDefault(collectable => collectable.Type == type);
    }
}

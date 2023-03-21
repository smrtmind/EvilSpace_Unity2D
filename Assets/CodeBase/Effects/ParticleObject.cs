using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Effects
{
    public class ParticleObject : MonoBehaviour
    {
        [field: SerializeField] public ParticleType Type { get; private set; }
        [field: SerializeField] public bool IsBusy { get; private set; }

        public void SetBusyState(bool state)
        {
            IsBusy = state;
            gameObject.SetActive(IsBusy);
        }
    }
}

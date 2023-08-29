using CodeBase.Service;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Effects
{
    public class ParticleObject : MonoBehaviour
    {
        [field: SerializeField] public ParticleType ParticleType { get; private set; }

        public bool IsBusy { get; private set; } = true;

        public void SetBusyState(bool state)
        {
            IsBusy = state;
            gameObject.SetActive(IsBusy);
        }
    }
}

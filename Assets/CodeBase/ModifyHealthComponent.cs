using UnityEngine;

namespace Scripts
{
    public class ModifyHealthComponent : MonoBehaviour
    {
        [SerializeField] private int _healthDelta;

        public void Apply(GameObject target)
        {
            var _healthComponent = target.GetComponent<HealthComponent>();
            if (_healthComponent != null)
            {
                _healthComponent.ModifyHealth(_healthDelta);
            }
        }
    }
}

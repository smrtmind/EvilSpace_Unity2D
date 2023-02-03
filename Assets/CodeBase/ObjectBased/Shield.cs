using System.Threading.Tasks;
using UnityEngine;

namespace CodeBase.ObjectBased
{
    public class Shield : MonoBehaviour
    {
        [SerializeField] private int activeDuration = 1;

        public async void Activate()
        {
            gameObject.SetActive(true);

            await Task.Delay(activeDuration * 1000);

            gameObject.SetActive(false);
        }
    }
}

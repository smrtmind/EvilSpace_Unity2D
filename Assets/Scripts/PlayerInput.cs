using UnityEngine;

namespace Scripts
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;

        private void Update()
        {
            _player.burst = Input.GetAxis("Vertical");
            _player.firstWeapon = Input.GetButton("Fire1");
            _player.secondWeapon = Input.GetButton("Fire2");
        }
    }
}

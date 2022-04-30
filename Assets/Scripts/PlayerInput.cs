using UnityEngine;

namespace Scripts
{
    public class PlayerInput : MonoBehaviour
    {
        [SerializeField] private PlayerController _player;

        private void Update()
        {
            _player.burst = Input.GetAxis("Vertical");
            _player.leftTurn = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            _player.rightTurn = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

            _player.firstWeapon = Input.GetButton("Fire1");
            _player.secondWeapon = Input.GetButton("Fire2");
            _player.thirdWeapon = Input.GetButton("Fire3");
        }
    }
}

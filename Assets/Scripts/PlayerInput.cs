using UnityEngine;

namespace Scripts
{
    public class PlayerInput : MonoBehaviour
    {
        private PlayerController _player;

        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();
        }

        //private void Update()
        //{
        //    for pc build

        //    _player.burst = Input.GetAxis("Vertical");
        //    _player.leftTurn = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
        //    _player.rightTurn = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);

        //    _player.firstWeapon = Input.GetButton("Fire1");
        //    _player.secondWeapon = Input.GetButton("Fire2");
        //    _player.thirdWeapon = Input.GetButton("Fire3");
        //}

        public void WeaponGunShoot() => _player.firstWeapon = true;
        public void WeaponGunStop() => _player.firstWeapon = false;

        public void WeaponBlasterShoot() => _player.secondWeapon = true;
        public void WeaponBlasterStop() => _player.secondWeapon = false;

        public void WeaponBombShoot() => _player.thirdWeapon = true;
        public void WeaponBombStop() => _player.thirdWeapon = false;
    }
}

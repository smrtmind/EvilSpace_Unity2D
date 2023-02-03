using UnityEngine;

namespace CodeBase.Player
{
    public class PlayerInput : MonoBehaviour
    {
        private Joystick joystick;

        private void OnEnable()
        {
            joystick = FindObjectOfType<Joystick>();
        }

        public float Direction => joystick.Horizontal;
        public bool LeftTurn => Direction <= -0.5f;
        public bool RightTurn => Direction >= 0.5f;
    }
}

using UnityEngine;

namespace Scripts
{
    public class GalaxyRotation : MonoBehaviour
    {
        [SerializeField] private float rotationSpeed = 5f;

        private Rigidbody2D _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _body.AddTorque(rotationSpeed);
        }
    }
}

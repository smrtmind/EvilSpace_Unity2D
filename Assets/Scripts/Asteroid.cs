using UnityEngine;

namespace Scripts
{
    public class Asteroid : MonoBehaviour
    {
        [SerializeField] private float _minSpeed = 1f;
        [SerializeField] private float _maxSpeed = 5f;

        private Rigidbody2D _body;
        private CircleCollider2D _collider;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            _collider = GetComponent<CircleCollider2D>();
        } 
    }
}

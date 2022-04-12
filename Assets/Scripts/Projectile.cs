using UnityEngine;

namespace Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float _speed = 5f;

        private Rigidbody2D _body;

        private void Awake()
        {
            _body = GetComponent<Rigidbody2D>();
            
            //destroy laser after 5 sec
            Destroy(gameObject, 5f);
        }

        public void Launch(Vector2 velocity, Vector2 direction)
        {
            _body.velocity = velocity + direction * _speed;
        }
    }
}

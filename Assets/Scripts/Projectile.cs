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
            
            //destroy laser after 2 sec
            Destroy(gameObject, 2f);
        }

        public void Launch(Vector2 velocity, Vector2 direction)
        {
            _body.velocity = velocity + direction * _speed;
        }
    }
}

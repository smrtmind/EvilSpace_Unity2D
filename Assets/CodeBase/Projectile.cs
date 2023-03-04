using UnityEngine;

namespace Scripts
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed = 5f;
        [SerializeField] private bool isHostile;
        public bool IsHostile => isHostile;
        public bool IsBusy { get; private set; }

        void Update()
        {
            // Get the screen boundaries in world coordinates
            Vector2 screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

            // Check if the object is out of the screen boundaries
            if (transform.position.y > screenBoundaries.y || transform.position.y < -screenBoundaries.y
            || transform.position.x > screenBoundaries.x || transform.position.x < -screenBoundaries.x)
            {
                SetBusyState(false);
            }
        }

        public void SetBusyState(bool isBusy)
        {
            IsBusy = isBusy;
            gameObject.SetActive(isBusy);
        }

        public void Launch(Vector2 velocity, Vector2 direction)
        {
            rb.velocity = velocity + direction * speed;
        }
    }
}

using System.Collections;
using UnityEngine;

namespace CodeBase.ObjectBased
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float speed = 5f;

        public bool IsBusy { get; private set; }

        private Vector2 screenBoundaries;

        private void OnEnable()
        {
            StartCoroutine(CheckForScreenBounds());
        }

        private IEnumerator CheckForScreenBounds()
        {
            while (true)
            {
                yield return new WaitForEndOfFrame();

                screenBoundaries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

                if (transform.position.y > screenBoundaries.y || transform.position.y < -screenBoundaries.y
                || transform.position.x > screenBoundaries.x || transform.position.x < -screenBoundaries.x)
                {
                    SetBusyState(false);
                    break;
                }
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

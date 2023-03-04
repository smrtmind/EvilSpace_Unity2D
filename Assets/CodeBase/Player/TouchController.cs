using UnityEngine;

namespace CodeBase.Player
{
    public class TouchController : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float moveSpeed = 10f;

        private Vector3 mousePosition;
        private Vector3 direction;

        private float minX;
        private float maxX;
        private float minY;
        private float maxY;

        private void OnEnable()
        {
            CalculateScreenBounds();
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                direction = (mousePosition - transform.position);

                Vector3 constrainedPosition = transform.position + direction * moveSpeed * Time.deltaTime;
                constrainedPosition.x = Mathf.Clamp(constrainedPosition.x, minX, maxX);
                constrainedPosition.y = Mathf.Clamp(constrainedPosition.y, minY, maxY);
                transform.position = constrainedPosition;

                rb.velocity = Vector2.zero;
            }
        }

        private void CalculateScreenBounds()
        {
            Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z));
            Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.transform.position.z));

            minX = bottomLeft.x;
            maxX = topRight.x;
            minY = bottomLeft.y;
            maxY = topRight.y;
        }
    }
}

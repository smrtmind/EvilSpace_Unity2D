using CodeBase.Animation;
using CodeBase.Utils;
using System;
using UnityEngine;

namespace CodeBase.Player
{
    public class TouchController : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        [Header("Player Settings")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private PlayerAnimationController playerAnimationController;

        public static Action<bool> OnStartMoving;

        private Vector3 mousePosition;
        private Vector3 direction;
        private bool isMoving;
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;

        private void OnEnable()
        {
            CalculateScreenBounds();
            dependencyContainer.TouchController = this;
            //enabled = false;
        }

        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (!isMoving)
                {
                    isMoving = true;
                    OnStartMoving?.Invoke(isMoving);
                }

                mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                direction = (mousePosition - transform.position);

                Vector3 constrainedPosition = transform.position + direction * movementSpeed * Time.deltaTime;
                constrainedPosition.x = Mathf.Clamp(constrainedPosition.x, minX, maxX);
                constrainedPosition.y = Mathf.Clamp(constrainedPosition.y, minY, maxY);
                transform.position = constrainedPosition;

                rb.velocity = Vector2.zero;

            }
            else
            {
                if (isMoving)
                {
                    isMoving = false;
                    OnStartMoving?.Invoke(isMoving);
                }

                playerAnimationController.UpdateAnimation(0f);
            }

            if (Input.GetMouseButton(0) && Mathf.Abs(direction.x) > 0.5f)
            {
                Vector3 directionX = new Vector3(direction.x, 0, 0);
                if (directionX.magnitude > 0.5f)
                {
                    playerAnimationController.UpdateAnimation(direction.x);
                }
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

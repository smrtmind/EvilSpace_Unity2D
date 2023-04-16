using CodeBase.Animation;
using CodeBase.Service;
using System;
using UnityEngine;
using Zenject;

namespace CodeBase.Player
{
    public class TouchController : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private PlayerStorage playerStorage;

        [Header("Player Settings")]
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private PlayerAnimationController playerAnimationController;

        public static Action<bool> OnStartMoving;

        private Vector3 mousePosition;
        private Vector3 direction;
        private bool isMoving;
        private float minX;
        private float maxX;
        private float minY;
        private float maxY;
        private Camera mainCamera;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            mainCamera = cameraController.MainCamera;
        }

        private void OnEnable()
        {
            CalculateScreenBounds();
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

                mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;
                direction = (mousePosition - transform.position);

                Vector3 constrainedPosition = transform.position + direction * playerStorage.ConcretePlayer.MovementSpeed * Time.deltaTime;
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
            }

            if (Input.GetMouseButton(0) && Mathf.Abs(direction.x) > playerAnimationController.TurnDetectIndent)
            {
                Vector3 directionX = new Vector3(direction.x, 0, 0);
                if (directionX.magnitude > playerAnimationController.TurnDetectIndent)
                {
                    playerAnimationController.UpdateAnimation(direction.x);
                }
            }
            else
            {
                playerAnimationController.UpdateAnimation(0f);
            }

            if (Input.GetMouseButton(0) && Mathf.Abs(direction.y) > playerAnimationController.TurnDetectIndent)
            {
                Vector3 directionX = new Vector3(0, direction.y, 0);
                if (directionX.magnitude > playerAnimationController.TurnDetectIndent)
                {
                    playerAnimationController.ScaleFlame(direction.y);
                }
            }
            else
            {
                playerAnimationController.ScaleFlame(0f);
            }
        }

        private void CalculateScreenBounds()
        {
            Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.transform.position.z));
            Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.transform.position.z));

            minX = bottomLeft.x;
            maxX = topRight.x;
            minY = bottomLeft.y;
            maxY = topRight.y;
        }
    }
}

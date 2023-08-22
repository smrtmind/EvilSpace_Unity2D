using CodeBase.Service;
using System.Collections;
using UnityEngine;
using Zenject;
using static CodeBase.Utils.Enums;

namespace CodeBase.ObjectBased
{
    public class Planet : MonoBehaviour
    {
        [field: SerializeField] public PlanetType Type { get; private set; }
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField, Range(0f, 255f)] private float minShadeValue;
        [SerializeField, Range(0f, 255f)] private float maxShadeValue;
        [SerializeField, Range(1f, 10f)] private float minScale;
        [SerializeField, Range(1f, 10f)] private float maxScale;

        public bool IsBusy { get; private set; }

        private Camera mainCamera;
        private Vector2 screenBoundaries;
        private Vector3 mainCameraPosition;
        private Vector3 defaultPosition;
        private float movementSpeed;

        [Inject]
        private void Construct(CameraController cameraController)
        {
            mainCamera = cameraController.MainCamera;
            mainCameraPosition = cameraController.MainCamera.transform.position;
        }

        private void OnEnable()
        {
            CreateRandomVisual();
            StartCoroutine(Move());
        }

        private void Start()
        {
            defaultPosition = transform.position;
        }

        private void CreateRandomVisual()
        {
            float randomShade = Random.Range(minShadeValue / 255f, maxShadeValue / 255f);
            spriteRenderer.color = new Color(randomShade, randomShade, randomShade);

            float randomScale = Random.Range(minScale, maxScale);
            transform.localScale = new Vector3(randomScale, randomScale, 1f);
        }


        private IEnumerator Move()
        {
            while (CheckForScreenBounds())
            {
                transform.position = new Vector3(defaultPosition.x, transform.position.y - Time.deltaTime * movementSpeed, defaultPosition.z);
                yield return new WaitForEndOfFrame();
            }
        }

        private bool CheckForScreenBounds()
        {
            screenBoundaries = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, mainCameraPosition.z));

            if (transform.position.y < -screenBoundaries.y - 15f)
            {
                Release();
                return false;
            }

            return true;
        }

        public void SetSpeed(float speed) => movementSpeed = speed;

        public void Take()
        {
            IsBusy = true;
            gameObject.SetActive(true);
        }

        private void Release()
        {
            gameObject.SetActive(false);
            IsBusy = false;
        }
    }
}

using CodeBase.Player;
using CodeBase.Service;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CodeBase.Utils
{
    public class BackgroundController : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Vector2 edgeScreenPadding = new Vector2(2f, 2f);

        [Header("Skybox settings")]
        [SerializeField] private Color colorOnHit;
        [SerializeField] private Color colorOnBombExplosion;

        [Space]
        [SerializeField] private float hitEffectDuration;

        private Color defaultColor;
        private bool isChangedColor;
        private Sequence playerCollisionBehaviour;
        private Coroutine explosionRoutine;
        private WeaponController weaponController;
        private Tween explosionTween;
        private Camera mainCamera;

        [Inject]
        private void Construct(CameraController cameraController, WeaponController weapon)
        {
            mainCamera = cameraController.MainCamera;
            weaponController = weapon;
        }

        private void OnEnable()
        {
            EventObserver.OnPlayerHit += ChangeSkyboxColor;
            EventObserver.OnBombButtonPressed += ImitateBombExplosionSkyBox;
            EventObserver.OnGameRestarted += ResetSkyBoxColor;
        }

        private void OnDisable()
        {
            EventObserver.OnPlayerHit -= ChangeSkyboxColor;
            EventObserver.OnBombButtonPressed -= ImitateBombExplosionSkyBox;
            EventObserver.OnGameRestarted -= ResetSkyBoxColor;
        }

        private void Start()
        {
            defaultColor = spriteRenderer.color;
            spriteRenderer.enabled = true;

            ScaleBackground();
        }

        private void ScaleBackground()
        {
            float screenHeight = mainCamera.orthographicSize * 2f;
            float screenWidth = screenHeight * Screen.width / Screen.height;

            Vector3 spriteSize = spriteRenderer.bounds.size;
            Vector3 newScale = transform.localScale;

            float scaleX = (screenWidth + edgeScreenPadding.x) / spriteSize.x;
            float scaleY = (screenHeight + edgeScreenPadding.y) / spriteSize.y;

            newScale.x = scaleX;
            newScale.y = scaleY;

            transform.localScale = newScale;
        }

        private void ResetSkyBoxColor() => mainCamera.backgroundColor = defaultColor;

        private void ChangeSkyboxColor()
        {
            if (!isChangedColor)
            {
                isChangedColor = true;

                playerCollisionBehaviour = DOTween.Sequence().SetAutoKill(true);
                playerCollisionBehaviour.Append(spriteRenderer.DOColor(colorOnHit, hitEffectDuration / 2f))
                                        .Append(spriteRenderer.DOColor(defaultColor, hitEffectDuration / 2f))
                                        .OnComplete(() => isChangedColor = false);
            }
        }

        private void ImitateBombExplosionSkyBox()
        {
            if (explosionRoutine == null)
                explosionRoutine = StartCoroutine(Imitate());
        }

        private IEnumerator Imitate()
        {
            explosionTween?.Kill();
            explosionTween = spriteRenderer.DOColor(colorOnBombExplosion, weaponController.BombEffectDuration / 2f);

            var duration = weaponController.BombEffectDuration;
            while (duration > 0f)
            {
                yield return duration -= Time.deltaTime;
            }

            explosionTween?.Kill();
            explosionTween = spriteRenderer.DOColor(defaultColor, weaponController.BombEffectDuration / 2f);

            explosionRoutine = null;
        }
    }
}

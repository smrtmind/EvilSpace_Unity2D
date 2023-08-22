using CodeBase.Player;
using CodeBase.Utils;
using DG.Tweening;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CodeBase.Service
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }

        [Header("Skybox settings")]
        [SerializeField] private Color colorOnHit;
        [SerializeField] private Color colorOnBombExplosion;

        [Space]
        [SerializeField] private float effectDuration;

        private Color defaultColor;
        private bool isChangedColor;
        private Sequence playerCollisionBehaviour;
        private Coroutine explosionRoutine;
        private WeaponController weaponController;
        private Tween explosionTween;

        [Inject]
        private void Construct(WeaponController weapon)
        {
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
            defaultColor = MainCamera.backgroundColor;
        }

        private void ResetSkyBoxColor() => MainCamera.backgroundColor = defaultColor;

        private void ChangeSkyboxColor()
        {
            if (!isChangedColor)
            {
                isChangedColor = true;

                playerCollisionBehaviour = DOTween.Sequence().SetAutoKill(true);
                playerCollisionBehaviour.Append(MainCamera.DOColor(colorOnHit, effectDuration / 2f))
                                        .Append(MainCamera.DOColor(defaultColor, effectDuration / 2f))
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
            explosionTween = MainCamera.DOColor(colorOnBombExplosion, weaponController.BombEffectDuration / 2f);

            var duration = weaponController.BombEffectDuration;
            while (duration > 0f)
            {
                yield return duration -= Time.deltaTime;
            }

            explosionTween?.Kill();
            explosionTween = MainCamera.DOColor(defaultColor, weaponController.BombEffectDuration / 2f);

            explosionRoutine = null;
        }
    }
}

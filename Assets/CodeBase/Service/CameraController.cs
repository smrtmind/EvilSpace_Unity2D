using CodeBase.Utils;
using DG.Tweening;
using UnityEngine;

namespace CodeBase.Service
{
    public class CameraController : MonoBehaviour
    {
        [field: SerializeField] public Camera MainCamera { get; private set; }

        [Header("Player hit settings")]
        [SerializeField] private Color skyBoxColor;
        [SerializeField] private float effectDuration;

        private Color defaultColor;
        private bool isChangedColor;
        private Sequence playerCollisionBehaviour;

        private void OnEnable()
        {
            EventObserver.OnPlayerHit += ChangeSkyboxColor;
        }

        private void OnDisable()
        {
            EventObserver.OnPlayerHit -= ChangeSkyboxColor;
        }

        private void Start()
        {
            defaultColor = MainCamera.backgroundColor;
        }

        private void ChangeSkyboxColor()
        {
            if (!isChangedColor)
            {
                isChangedColor = true;

                playerCollisionBehaviour = DOTween.Sequence().SetAutoKill(true);
                playerCollisionBehaviour.Append(MainCamera.DOColor(skyBoxColor, effectDuration / 2f))
                                        .Append(MainCamera.DOColor(defaultColor, effectDuration / 2f))
                                        .OnComplete(() => isChangedColor = false);
            }
        }
    }
}

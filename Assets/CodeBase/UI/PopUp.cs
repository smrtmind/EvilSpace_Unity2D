using CodeBase.Utils;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace CodeBase.UI
{
    public class PopUp : MonoBehaviour
    {
        [Header("Storages")]
        [SerializeField] private DependencyContainer dependencyContainer;

        [field: SerializeField] public TextMeshProUGUI ValueInfo { get; private set; }
        [SerializeField] private Vector3 startingScale;

        private Transform targetPoint;
        private Vector3 defaultPopUpPosition;
        private Quaternion defaultPopUpRotation;
        private Color defaultPopUpColor;
        private bool popUpIsActive;

        private void OnEnable()
        {
            defaultPopUpPosition = transform.position;
            defaultPopUpRotation = transform.rotation;
            defaultPopUpColor = ValueInfo.color;
        }

        public void SetCurrentData(Transform point, string value, string color)
        {
            if (!popUpIsActive)
            {
                targetPoint = point;
                ValueInfo.text = $"<color={color}>{value}</color>";
            }
        }

        public void SpawnPopUp()
        {
            if (!popUpIsActive)
            {
                popUpIsActive = true;
                transform.SetParent(dependencyContainer.ParticlePool.PopUpContainer);
                transform.position = targetPoint.position;
                transform.localScale = Vector3.zero;
                transform.rotation = defaultPopUpRotation;
                gameObject.SetActive(true);

                Sequence popUpAppear = DOTween.Sequence().SetAutoKill();
                popUpAppear.Append(transform.DOScale(startingScale, 0.25f))
                           .Append(transform.DOScale(Vector3.one, 0.25f))
                           .OnComplete(() => FadePopUp());
            }
        }

        private void FadePopUp()
        {
            transform.DOMoveY(transform.position.y + 2f, 0.5f).SetEase(Ease.Linear).SetAutoKill();
            ValueInfo.DOColor(new Color(ValueInfo.color.r, ValueInfo.color.g, ValueInfo.color.b, 0f), 0.5f)
                     .OnComplete(() => RefreshPopUp()).SetAutoKill();
        }

        private void RefreshPopUp()
        {
            popUpIsActive = false;

            StopAllCoroutines();
            gameObject.SetActive(false);
            transform.position = defaultPopUpPosition;
            transform.rotation = defaultPopUpRotation;
            transform.localScale = Vector3.zero;
            ValueInfo.color = defaultPopUpColor;
        }
    }
}

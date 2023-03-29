using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Animation
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private SpriteRenderer skinRenderer;

        [field: SerializeField] public float TurnDetectIndent { get; private set; } = 0.5f;

        [Header("Flame Settings")]
        [SerializeField] private float scaleFlameDuration = 0.15f;
        [SerializeField] private float minFlameScale;
        [SerializeField] private float maxFlameScale;
        [SerializeField] private float averageFlameScale;
        [SerializeField] private List<Transform> starterFlames;
        [SerializeField] private List<SpriteRenderer> starterFlamesSkins;
        [SerializeField] private Transform criticalDamageFlame;
        [SerializeField] private Vector3 criticalDamageScale;
        [SerializeField] private List<StarterFlameParameters> flameParameters;

        private static readonly int MoveKey = Animator.StringToHash("direction");

        private Color defaultColor;
        private MovementState previousState;
        private bool flameIsScaling;
        private float currentScaleValue;
        private float previousScaleValue;
        private Vector3 newFlameScale;
        private Sequence criticalBehaviour;
        private Tween criticalDamageFlameTween;

        private void Start()
        {
            defaultColor = skinRenderer.color;
        }

        public void UpdateAnimation(float directionX)
        {
            animator.SetFloat(MoveKey, directionX);

            UpdateStraterFlames(directionX);
        }

        private void UpdateStraterFlames(float directionX)
        {
            if (directionX < TurnDetectIndent && directionX != 0f)
                ChangeFlamePosition(MovementState.Left);
            else if (directionX > TurnDetectIndent && directionX != 0f)
                ChangeFlamePosition(MovementState.Right);
            else
                ChangeFlamePosition(MovementState.Moveless);
        }

        private void ChangeFlamePosition(MovementState state)
        {
            if (state != previousState)
            {
                previousState = state;
                var currentPoints = GetCurrentFlameParameters(state);

                for (int i = 0; i < starterFlames.Count; i++)
                    starterFlames[i].position = currentPoints.StarterFlamesPoints[i].position;

                criticalDamageFlame.position = currentPoints.DamageFlamePoint.position;
            }
        }

        private StarterFlameParameters GetCurrentFlameParameters(MovementState state)
        {
            foreach (var points in flameParameters)
            {
                if (points.MovementState == state)
                {
                    return points;
                }
            }

            return null;
        }

        public void ScaleFlame(float directionY)
        {
            if (directionY < TurnDetectIndent && directionY != 0f)
                currentScaleValue = minFlameScale;
            else if (directionY > TurnDetectIndent && directionY != 0f)
                currentScaleValue = maxFlameScale;
            else
                currentScaleValue = averageFlameScale;

            if (currentScaleValue != previousScaleValue && !flameIsScaling)
            {
                previousScaleValue = currentScaleValue;
                flameIsScaling = true;

                foreach (Transform flamePivot in starterFlames)
                {
                    newFlameScale = new Vector3(flamePivot.localScale.x, currentScaleValue, flamePivot.localScale.z);
                    flamePivot.DOScale(newFlameScale, scaleFlameDuration).OnComplete(() => flameIsScaling = false);
                }
            }
        }

        public void EnableCriticalDamageVisual(bool enable)
        {
            if (enable && !criticalDamageFlame.gameObject.activeSelf)
            {
                criticalDamageFlame.localScale = Vector3.zero;
                criticalDamageFlame.gameObject.SetActive(true);

                criticalDamageFlameTween?.Kill();
                criticalDamageFlameTween = criticalDamageFlame.DOScale(criticalDamageScale, 2f);

                criticalBehaviour = DOTween.Sequence().SetAutoKill(true);
                criticalBehaviour.Append(skinRenderer.DOColor(Color.red, 0.1f))
                                 .Append(skinRenderer.DOColor(defaultColor, 0.1f))
                                 .SetLoops(-1);
            }
            else if (!enable)
            {
                criticalDamageFlame.gameObject.SetActive(false);

                if (criticalBehaviour != null)
                    criticalBehaviour.Kill();

                skinRenderer.color = defaultColor;
            }
        }

        public void EnableStarterFlames(bool enable)
        {
            foreach (var flameSkin in starterFlamesSkins)
                flameSkin.enabled = enable;
        }
    }

    [Serializable]
    public class StarterFlameParameters
    {
        [field: SerializeField] public MovementState MovementState { get; private set; }
        [field: SerializeField] public List<Transform> StarterFlamesPoints { get; private set; }
        [field: SerializeField] public Transform DamageFlamePoint { get; private set; }
    }
}

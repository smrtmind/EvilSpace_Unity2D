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
        [field: SerializeField] public float TurnDetectIndent { get; private set; } = 0.5f;

        [Header("Flame Settings")]
        [SerializeField] private float scaleFlameDuration = 0.15f;
        [SerializeField] private float minFlameScale;
        [SerializeField] private float maxFlameScale;
        [SerializeField] private float averageFlameScale;
        [SerializeField] private List<Transform> starterFlames;
        [SerializeField] private List<StarterFlameParameters> flameParameters;

        private static readonly int MoveKey = Animator.StringToHash("direction");

        private MovementState previousState;
        private bool flameIsScaling;
        private float currentScaleValue;
        private float previousScaleValue;
        private Vector3 newFlameScale;

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
                var currentPoints = GetCurrentFlamePoints(state);

                for (int i = 0; i < starterFlames.Count; i++)
                    starterFlames[i].position = currentPoints[i].position;
            }
        }

        private List<Transform> GetCurrentFlamePoints(MovementState state)
        {
            foreach (var points in flameParameters)
            {
                if (points.MovementState == state)
                {
                    return points.StarterFlamesPoints;
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
    }

    [Serializable]
    public class StarterFlameParameters
    {
        [field: SerializeField] public MovementState MovementState { get; private set; }
        [field: SerializeField] public List<Transform> StarterFlamesPoints { get; private set; }
    }
}

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
        [SerializeField] private float scaleFlameOnY = 0.15f;
        [SerializeField] private List<Transform> starterFlames;
        [SerializeField] private List<StarterFlameParameters> flameParameters;

        private static readonly int MoveKey = Animator.StringToHash("direction");

        private MovementState previousState;
        private bool flameIsScaling;

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
            if (previousState == state) return;

            previousState = state;
            var currentPoints = GetCurrentFlamePoints(state);

            for (int i = 0; i < starterFlames.Count; i++)
                starterFlames[i].position = currentPoints[i].position;
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
            if (!flameIsScaling)
            {
                flameIsScaling = true;

                if (directionY < TurnDetectIndent && directionY != 0f)
                {
                    foreach (Transform flamePivot in starterFlames)
                        flamePivot.DOScale(new Vector3(flamePivot.localScale.x, 0.5f, flamePivot.localScale.z), scaleFlameOnY).OnComplete(() => flameIsScaling = false);
                }
                else if (directionY > TurnDetectIndent && directionY != 0f)
                {
                    foreach (Transform flamePivot in starterFlames)
                        flamePivot.DOScale(new Vector3(flamePivot.localScale.x, 2f, flamePivot.localScale.z), scaleFlameOnY).OnComplete(() => flameIsScaling = false);
                }
                else
                {
                    foreach (Transform flamePivot in starterFlames)
                        flamePivot.DOScale(new Vector3(flamePivot.localScale.x, 1f, flamePivot.localScale.z), scaleFlameOnY).OnComplete(() => flameIsScaling = false);
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

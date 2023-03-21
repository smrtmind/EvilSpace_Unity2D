using System;
using System.Collections.Generic;
using UnityEngine;
using static CodeBase.Utils.Enums;

namespace CodeBase.Animation
{
    public class PlayerAnimationController : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private List<GameObject> starterFlames;
        [SerializeField] private List<StarterFlameParameters> flameParameters;

        private static readonly int MoveKey = Animator.StringToHash("direction");

        public void UpdateAnimation(float directionX)
        {
            animator.SetFloat(MoveKey, directionX);

            UpdateStraterFlames(directionX);
        }

        private void UpdateStraterFlames(float directionX)
        {
            if (directionX < 0.5f && directionX != 0f)
                ChangeFlamePosition(MovementState.Left);
            else if (directionX > 0.5f && directionX != 0f)
                ChangeFlamePosition(MovementState.Right);
            else
                ChangeFlamePosition(MovementState.Straight);
        }

        private void ChangeFlamePosition(MovementState state)
        {
            var currentPoints = GetCurrentFlamePoints(state);

            for (int i = 0; i < starterFlames.Count; i++)
                starterFlames[i].transform.position = currentPoints[i].position;
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
    }

    [Serializable]
    public class StarterFlameParameters
    {
        [field: SerializeField] public MovementState MovementState { get; private set; }
        [field: SerializeField] public List<Transform> StarterFlamesPoints { get; private set; }
    }
}

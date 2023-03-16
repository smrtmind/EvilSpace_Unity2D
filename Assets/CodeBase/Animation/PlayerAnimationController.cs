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
            {
                starterFlames[0].transform.position = flameParameters[0].StarterFlamesPoints[0].position;
                starterFlames[1].transform.position = flameParameters[0].StarterFlamesPoints[1].position;
            }
            else if (directionX > 0.5f && directionX != 0f)
            {
                starterFlames[0].transform.position = flameParameters[2].StarterFlamesPoints[0].position;
                starterFlames[1].transform.position = flameParameters[2].StarterFlamesPoints[1].position;
            }
            else
            {
                starterFlames[0].transform.position = flameParameters[1].StarterFlamesPoints[0].position;
                starterFlames[1].transform.position = flameParameters[1].StarterFlamesPoints[1].position;
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

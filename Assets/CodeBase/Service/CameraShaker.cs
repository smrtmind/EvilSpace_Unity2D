using CodeBase.Utils;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Service
{
    [RequireComponent(typeof(Camera))]
    public class CameraShaker : MonoBehaviour
    {
        private Vector3 defaultPosition;
        private Coroutine shakeRoutine;

        private void OnEnable()
        {
            EventObserver.OnShakeCamera += ShakeCamera;
        }

        private void OnDisable()
        {
            EventObserver.OnShakeCamera -= ShakeCamera;
        }

        private void Start()
        {
            defaultPosition = transform.position;
        }

        private void ShakeCamera(float duration, float strength)
        {
            if (shakeRoutine == null)
                shakeRoutine = StartCoroutine(PerformShake(duration, strength));
        }

        private IEnumerator PerformShake(float duration, float strength)
        {
            var animationDuration = 0f;

            while (animationDuration < duration)
            {
                animationDuration += Time.deltaTime;
                Vector3 delta = Random.insideUnitCircle.normalized * strength;
                transform.position = defaultPosition + delta;

                yield return new WaitForEndOfFrame();
            }

            transform.position = defaultPosition;
            shakeRoutine = null;
        }
    }
}

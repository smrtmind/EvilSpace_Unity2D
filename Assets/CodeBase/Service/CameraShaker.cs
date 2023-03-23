using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace CodeBase.Service
{
    [RequireComponent(typeof(Camera))]
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float duration = 0.2f;
        [SerializeField] private float maxDelta = 0.1f;

        public static Action OnShakeCamera;

        private Vector3 defaultPosition;
        private Coroutine shakeCoroutine;
        private float animationDuration;
        //private float defaultDuration;
        //private float defaultMaxDelta;

        //private void Awake()
        //{
        //    defaultPosition = transform.position;
        //    //defaultDuration = duration;
        //    //defaultMaxDelta = maxDelta;
        //}

        private void OnEnable()
        {
            defaultPosition = transform.position;

            OnShakeCamera += ShakeCamera;
        }

        private void OnDisable()
        {
            OnShakeCamera -= ShakeCamera;
        }

        private void ShakeCamera()
        {
            if (shakeCoroutine != null)
                StopCoroutine(shakeCoroutine);

            shakeCoroutine = StartCoroutine(StartAnimation());
        }

        public void SetDuration(float value) => duration = value;
        public void SetMaxDelta(float value) => maxDelta = value;

        private IEnumerator StartAnimation()
        {
            animationDuration = 0;

            while (animationDuration < duration)
            {
                animationDuration += Time.deltaTime;
                Vector3 delta = Random.insideUnitCircle.normalized * maxDelta;
                transform.position = defaultPosition + delta;

                yield return null;
            }

            transform.position = defaultPosition;
        }

        //public void RestoreValues()
        //{
        //    duration = defaultDuration;
        //    maxDelta = defaultMaxDelta;
        //}
    }
}

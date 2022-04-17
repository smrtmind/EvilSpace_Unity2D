using System.Collections;
using UnityEngine;


namespace Scripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private float _maxDelta = 0.1f;

        private Vector3 _defaultPosition;
        private float _animationDuration;
        private Coroutine _coroutine;

        private void Awake()
        {
            _defaultPosition = transform.position;
        }

        public void ShakeCamera()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartAnimation());
        }

        private IEnumerator StartAnimation()
        {
            _animationDuration = 0;

            while (_animationDuration < _duration)
            {
                _animationDuration += Time.deltaTime;
                Vector3 delta = Random.insideUnitCircle.normalized * _maxDelta;
                transform.position = _defaultPosition + delta;

                yield return null;
            }

            transform.position = _defaultPosition;
        }
    }
}

using System.Collections;
using UnityEngine;


namespace CodeBase.Service
{
    [RequireComponent(typeof(Camera))]
    public class CameraShaker : MonoBehaviour
    {
        [SerializeField] private float _duration = 0.2f;
        [SerializeField] private float _maxDelta = 0.1f;

        private Vector3 _defaultPosition;
        private float _animationDuration;
        private Coroutine _coroutine;
        private float _defaultDuration;
        private float _defaultMaxDelta;

        private void Awake()
        {
            _defaultPosition = transform.position;
            _defaultDuration = _duration;
            _defaultMaxDelta = _maxDelta;   
        }

        public void ShakeCamera()
        {
            if (_coroutine != null)
                StopCoroutine(_coroutine);

            _coroutine = StartCoroutine(StartAnimation());
        }

        public void SetDuration(float value) => _duration = value;
        public void SetMaxDelta(float value) => _maxDelta = value;

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

        public void RestoreValues()
        {
            _duration = _defaultDuration;
            _maxDelta = _defaultMaxDelta;
        }
    }
}

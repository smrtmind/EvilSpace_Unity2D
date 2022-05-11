using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace Scripts
{
    public class TimerComponent : MonoBehaviour
    {
        [SerializeField] private TimerData[] _timers;

        public void SetTimer(int index)
        {
            var timer = _timers[index];
            StartCoroutine(StartTimer(timer));
        }

        private IEnumerator StartTimer(TimerData timer)
        {
            yield return new WaitForSeconds(timer.Delay);

            timer.OnTimesUp?.Invoke();
        }

        [Serializable]
        public class TimerData
        {
            [SerializeField] private string _name;

            public float Delay;
            public UnityEvent OnTimesUp;
        }
    }
}

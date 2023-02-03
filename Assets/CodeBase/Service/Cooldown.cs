using System;
using UnityEngine;

namespace CodeBase.Service
{
    [Serializable]
    public class Cooldown
    {
        [SerializeField] private float _value;

        private float _timeIsUp;

        public float Value
        {
            get => _value;
            set => _value = value;
        }

        public void Reset()
        {
            _timeIsUp = Time.time + _value;
        }

        public bool IsReady => _timeIsUp <= Time.time;
    }
}

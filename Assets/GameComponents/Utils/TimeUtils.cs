using System;
using UnityEngine;

namespace GameComponents.Utils
{
    public static class TimeUtils
    {
        private static float _currentTime;
        
        public static void SetInterval(Action callback, float callsPerSec)
        {
            float timeToRayCast = 1 / callsPerSec;
        
            if (_currentTime <= Time.time)
            {
                _currentTime = Time.time + timeToRayCast;
                callback?.Invoke();
            }
        }
    }
}
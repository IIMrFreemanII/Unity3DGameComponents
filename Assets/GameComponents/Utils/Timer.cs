using UnityEngine;

namespace GameComponents.Utils
{
    public class Timer
    {
        private float currentTime;
        public bool finished;

        public void Tick()
        {
            if (currentTime <= 0)
            {
                finished = true;
                return;
            }
            
            currentTime -= Time.deltaTime;
        }

        public void SetTime(float time)
        {
            currentTime = time;
            finished = false;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Shared.Data
{
    class StateTimer
    {
        float remainingTime;
        bool done = false;
        public void Start(float _time)
        {
            Stop();
            remainingTime = _time;
            done = false;
            Shared.Manager.UpdateManager.GetInstance().OnUpdate += UpdateTimer;
        }

        public void Reset()
        {
            done = false;
            Stop();
        }

        void Stop()
        {
            Shared.Manager.UpdateManager.GetInstance().OnUpdate -= UpdateTimer;
        }

        public float GetRemainingTime()
        {
            return remainingTime;
        }

        public bool IsDone()
        {
            return done;
        }

        void UpdateTimer(float _delta)
        {
            remainingTime -= _delta;
            if (remainingTime <= 0.0f)
            {
                done = true;
                Stop();
            }

        }

    }
}

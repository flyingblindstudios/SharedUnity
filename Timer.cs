using Shared.Manager;
using System;
namespace Shared.Core
{
    public class Timer
    {
        float cTime;
        float tTime;
        bool isRunning = false;
        Action onDone = null;

        public Timer(float runTime)
        {
            tTime = runTime;
        }

        public Timer(float runTime, Action _onDone)
        {
            tTime = runTime;
            onDone = _onDone;
        }

        public void Start()
        {
            Reset();
            isRunning = true;
            UpdateManager.GetInstance().OnUpdate += UpdateTimer;
        }

        public void Reset()
        {
            Stop();
            cTime = 0.0f;
        }

        public void Stop()
        {
            isRunning = false;

            UpdateManager.GetInstance().OnUpdate -= UpdateTimer;
        }

        public void UpdateTimer(float _time)
        {
            cTime += _time;
            if (IsDone())
            {
                Stop();
                onDone?.Invoke();
            }
        }

        public bool IsDone()
        {
            if (cTime >= tTime)
            {
                return true;
            }
            return false;
        }

        public bool IsRunning()
        {
            return isRunning;
        }
    }
}
using Shared.Manager;

namespace Shared.Core
{
    public class Timer
    {
        float cTime;
        float tTime;
        bool isRunning = false;
        public Timer(float runTime)
        {
            tTime = runTime;
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
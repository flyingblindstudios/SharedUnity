using System;

namespace Shared.Manager
{
    public class PeriodicJob : Job
    {
        float m_PassedTime = 0.0f;
        float m_Rate;

        public PeriodicJob(Action<Job, float> _callback, float _updateRate) : base(_callback)
        {
            m_Rate = _updateRate;
        }

        public override void UpdateJob(float _deltaTime)
        {
            if (m_PassedTime >= m_Rate)
            {

                m_Callback(this, m_PassedTime);
                m_PassedTime = 0.0f;
            }


            m_PassedTime += _deltaTime;
        }
    }
}

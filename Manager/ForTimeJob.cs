using System;

namespace Shared.Manager
{
    public class ForTimeJob : Job
    {
        float m_PassedTime = 0.0f;
        float m_Time;
        public ForTimeJob(Action<Job, float> _callback, float _time) : base(_callback)
        {
            m_Time = _time;
        }

        public override void UpdateJob(float _deltaTime)
        {

            
            if (m_PassedTime >= m_Time)
            {
                
                Done();
                return;
            }

            m_Callback(this, _deltaTime);
            m_PassedTime += _deltaTime;
        }
    }
}

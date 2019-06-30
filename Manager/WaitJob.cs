using System;

/*
 * Waits and executes
 */
namespace Shared.Manager
{
    public class WaitJob : Job
    {
        float m_PassedTime = 0.0f;
        float m_WaitTime;
        public WaitJob(Action<Job, float> _callback, float _waitTime) : base(_callback)
        {
            m_WaitTime = _waitTime;
        }

        public override void UpdateJob(float _deltaTime)
        {
            
            if (m_PassedTime >= m_WaitTime)
            {
                m_Callback(this, m_PassedTime);
                Done();
                return;
            }
            m_PassedTime += _deltaTime;
        }
    }
}
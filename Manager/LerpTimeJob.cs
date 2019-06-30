using System;
using UnityEngine;
/*
 * Executes every frame, returns not delta time but normalized time
 */
namespace Shared.Manager
{
    public class LerpTimeJob : Job
    {

        float m_PassedTime = 0.0f;
        float m_LerpTime;
        bool m_FinishOnOne = false;
        public LerpTimeJob(Action<Job, float> _callback, float _lerpTime, bool _FinishOnOne = false) : base(_callback)
        {
            m_LerpTime = _lerpTime;
            m_FinishOnOne = _FinishOnOne;
        }

        public override void UpdateJob(float _deltaTime)
        {
            if (m_PassedTime >= m_LerpTime)
            {
                if (m_FinishOnOne)
                {
                    m_Callback(this, Mathf.Clamp01(m_PassedTime / m_LerpTime));
                }
                Done();
                return;
            }
            //deltat time
            m_Callback(this, Mathf.Clamp01( m_PassedTime/ m_LerpTime));

            m_PassedTime += _deltaTime;
            
        }
    }
}

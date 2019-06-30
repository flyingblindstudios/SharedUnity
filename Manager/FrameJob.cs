using System;

/*
 * Executes every frame
 */
namespace Shared.Manager
{
    public class FrameJob : Job
    {
        public FrameJob(Action<Job, float> _callback) : base(_callback)
        {
           
        }

        public override void UpdateJob(float _deltaTime)
        {
            m_Callback(this, _deltaTime);
        }
    }
}
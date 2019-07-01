using System;

namespace Shared.Manager
{

    /**
     * TODO: Implement a lazy job, there executed at some point
     * **/

    public abstract class LazyJob : Job
    {
        public LazyJob(Action<Job, float> _callback) : base(_callback)
        {

        }

        public override void UpdateJob(float _deltaTime)
        {
            m_Callback(this, _deltaTime);
            Done();
        }
    }
}
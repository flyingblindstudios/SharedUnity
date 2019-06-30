using System;

namespace Shared.Manager
{
    public abstract class Job
    {
        protected Action<Job, float> m_Callback;
        protected Action<Job> m_OnDone;
        protected Action<Job> m_OnCancle;

        protected bool m_Done = false;
        protected bool m_Started = false;

        protected Job(Action<Job, float> _callback )
        {
            m_Callback = _callback;
        }

        public void Start()
        {
            JopManager.StartJob(this);
        }

        public void Done()
        {
            JopManager.JobDone(this);
        }

        public void Cancle()
        {
            JopManager.CancleJob(this);
        }

        public abstract void UpdateJob(float _deltaTime);

        public virtual void OnStart()
        {
            m_Started = true;
        }

        public virtual void OnDone()
        {
            m_Done = true;
            m_OnDone?.Invoke(this);
        }

        public virtual void OnCancle()
        {
            m_OnCancle?.Invoke(this);
        }

        public bool IsDone()
        {
            return m_Done;
        }

        public bool IsStarted()
        {
            return m_Started;
        }
    }
}
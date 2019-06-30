using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Manager
{
    public class JopManager : MonoBehaviour
    {
        public static void StartJob( Job _job )
        {
            if (_job.IsStarted())
            {
                return;
            }

            _job.OnStart();
            UpdateManager.GetInstance().OnUpdate += _job.UpdateJob;
           
        }

        public static void CancleJob( Job _job )
        {
            _job.OnCancle();
            UpdateManager.GetInstance().OnUpdate -= _job.UpdateJob;
        }

        public static void JobDone(Job _job)
        {
            _job.OnDone();
            UpdateManager.GetInstance().OnUpdate -= _job.UpdateJob;
        }
    }
}

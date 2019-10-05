using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.AI
{ 
    public class GoapStateMachine : StateMachine
    {

        bool m_ReachedTarget = false;
        I_NavAgent m_NavAgent;
        public GoapStateMachine(I_NavAgent _navAgent)
        {
            m_NavAgent = _navAgent;
        }

        public override void UpdateState()
        {
            

            if (!m_ReachedTarget)
            {
                m_ReachedTarget = m_NavAgent.HasReachedDestination();
                return;
            }

            base.UpdateState();
        }

        protected override void NextState()
        {
            base.NextState();
            //if i am not done running check if we need to be close to our target
            if (!IsDone())
            { 
                I_GoapAction sm = (I_GoapAction)GetCurrentState();

                m_ReachedTarget = !sm.RequiresInRange();
                if(!m_ReachedTarget)
                {
                    Vector3 target = sm.GetTargetPosition();
                    m_NavAgent.SetTarget(target);
                    Debug.Log("Target position: " + target);
                }
            }
        }




    }
}
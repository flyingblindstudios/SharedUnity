using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.AI
{ 
    public class GoapStateMachine : StateMachine
    {
        enum State { NONE, EXECTUE_ACTION, GOTO_ACTION }
        State currentState = State.EXECTUE_ACTION;

    
        I_NavAgent m_NavAgent;
        public GoapStateMachine(I_NavAgent _navAgent)
        {
            m_NavAgent = _navAgent;
        }

        public override void UpdateState()
        {
            if (currentState == State.GOTO_ACTION)
            {
                bool reachedTarget = m_NavAgent.HasReachedDestination();
                if (reachedTarget)
                {
                    currentState = State.EXECTUE_ACTION;
                    EnterNextState();
                }
            }
            else if (currentState == State.EXECTUE_ACTION)
            {
                base.UpdateState();
            }
        }

        protected override void NextState()
        {

            GoToNextState();
       
            //if i am not done running check if we need to be close to our target
            if (!IsDone())
            {
                I_GoapAction sm = (I_GoapAction)GetCurrentState();

                bool inrange = sm.RequiresInRange();
                if (inrange)
                {
                    Vector3 target = sm.GetTargetPosition();
                    m_NavAgent.SetTarget(target);
                    currentState = State.GOTO_ACTION;
                    Debug.Log("GOTO ACTION");
                }
                else
                {
                    EnterNextState();
                    currentState = State.EXECTUE_ACTION;
                }
            }
            else
            {
                currentState = State.NONE;
            }
        }
    }
}
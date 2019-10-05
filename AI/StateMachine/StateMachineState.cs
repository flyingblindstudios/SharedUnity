using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.AI
{
    public class StateMachineState : I_StateMachineState
    {
        public bool m_CanBeInterrupted = false;
        public List<StateCondition> m_LoopConditions = new List<StateCondition>(); // for now only AND-conditions
        public bool m_Loop = false;
        public bool m_Break = false;

        public StateMachineState()
        {
            if (m_LoopConditions.Count > 0)
            {
                m_Loop = true;
            }
        }

        public virtual void Break() // if a state breaks, looping is ignored
        {
            m_Break = true;
        }

        public virtual void UpdateState()
        {

        }

        public virtual void OnStateEnter()
        {

        }

        public virtual void OnStateExit()
        {

        }

        public virtual void OnStateAbort()
        {

        }

        public virtual bool IsDone()
        {
            return false;
        }

        public virtual bool ShouldLoop()
        {
            if (m_LoopConditions.Count <= 0)
            {
                return m_Loop;
            }

            bool loop = true;
            for (int i = 0; i < m_LoopConditions.Count; i++)
            {
                loop = loop && m_LoopConditions[i].IsSatisfied();
            }

            return loop;
        }

        public virtual string GetDebugInfo()
        {
            return "";
        }

        public bool IsBreaking()
        {
            return m_Break;
        }
        public virtual void Reset()
        {
            m_Break = false;
        }

    };
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Shared.AI
{
    [Serializable]
    public class SharedGoapActionData
    {
        public int counter = 0;
    }

    public abstract class StateGoapActionConfig : GoapActionConfig, I_StateMachineState
    {
        [Header("Statemachine Settings")]
        public bool m_CanBeInterrupted = false;
        public List<StateCondition> m_LoopConditions = new List<StateCondition>(); // for now only AND-conditions
        public bool m_Loop = false;
        public bool m_Break = false;

        

        public virtual void Break()
        {
            m_Break = true;
        }

        public virtual string GetDebugInfo()
        {
            return name + "(" +sharedGoapData.counter.ToString()+ ")";
        }

        public abstract bool IsDone();

        public virtual void OnStateAbort()
        {
 
        }

        public virtual void OnStateEnter()
        {
            if (m_LoopConditions.Count > 0)
            {
                m_Loop = true;
            }
        }

        public virtual void OnStateExit()
        {
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

        public virtual void UpdateState()
        {
        }


        public override float GetCost()
        {
            return base.GetCost();
        }


        public bool IsBreaking()
        {
            return m_Break;
        }
        public virtual void Reset()
        {
            m_Break = false;
        }

        public override Vector3 GetTargetPosition()
        {
            return Vector3.zero;
        }

        public override void InitPlanning(I_GoapAgent _agent, Vector2 _pos)
        {
            base.InitPlanning(_agent, _pos);
        }

    }
}

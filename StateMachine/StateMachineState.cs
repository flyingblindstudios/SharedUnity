using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineState
{
    public bool m_CanBeInterrupted = false;
    public List<StateCondition> m_LoopConditions = new List<StateCondition>(); // for now only AND-conditions
    public bool m_Loop = false;
    public StateMachineState()
    {
        if (m_LoopConditions.Count > 0)
        {
            m_Loop = true;
        }
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

    public virtual bool IsDone()
    {
        return false;
    }

    public bool ShouldLoop()
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

}

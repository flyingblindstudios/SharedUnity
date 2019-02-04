using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class StateWaitTime : StateMachineState
{
    StateCondition m_Condition = null;

    float m_WaitTime = 0.0f;
    float m_PassedTime = 0.0f;

    public StateWaitTime(float _waitTime)
    {
        m_WaitTime = _waitTime;
    }

    public StateWaitTime(StateCondition _condition)
    {
        m_Condition = _condition;
    }

    public override void UpdateState()
    {
        if(m_Condition != null)
        {
            return;
        }
        m_PassedTime += Time.deltaTime;
    }

    public override void OnStateEnter()
    {
        m_PassedTime = 0.0f;
    }

    public override void OnStateExit()
    {
        m_PassedTime = 0.0f;
    }

    public override bool IsDone()
    {
        if (m_Condition != null)
        {
            return m_Condition.IsSatisfied();
        }

        return m_PassedTime >= m_WaitTime;
    }
}

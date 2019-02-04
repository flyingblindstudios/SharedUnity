using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StateMachine : StateMachineState
{

    List<StateMachineState> m_States = new List<StateMachineState>();
    int m_CurrentStateIndex = -1;

    public void AddStateAtFirst(StateMachineState _state)
    {
        m_States.Insert(0, _state);
    }

    public void AddState(StateMachineState _state)
    {
        m_States.Add(_state);
    }

    public override void UpdateState()
    {
        if (IsDone())
        {
            return;
        }

        if(m_CurrentStateIndex == -1)
        {
            m_CurrentStateIndex = 0;
            m_States[m_CurrentStateIndex].OnStateEnter();
        }

        if (m_States[m_CurrentStateIndex].IsDone())
        {
            m_States[m_CurrentStateIndex].OnStateExit();

            bool isLoopingState = m_States[m_CurrentStateIndex].ShouldLoop();

            if (!isLoopingState)
            {
                m_CurrentStateIndex++;
            }
            

            if (!this.IsDone()) // is statemachine done?
            {
                m_States[m_CurrentStateIndex].OnStateEnter();
            }
            else if (this.ShouldLoop()) // or loop condition
            {
                m_CurrentStateIndex = 0;
                m_States[m_CurrentStateIndex].OnStateEnter();
            }
            

            //if done, dont do anything, cause isDone will be true

        }
        else
        {
            m_States[m_CurrentStateIndex].UpdateState();
        }
    }

    public override void OnStateEnter()
    {

    }

    public override void OnStateExit()
    {

    }

    public override bool IsDone()
    {
        if(m_CurrentStateIndex >= m_States.Count)
        {
            return true;
        }
        return false;
    }

}

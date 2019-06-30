using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class StateMachine : StateMachineState
{



    /*
    * A State machine can loop states, but it cant loop itself!
    */

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

        if (m_States[m_CurrentStateIndex].IsDone() || m_States[m_CurrentStateIndex].m_Break)
        {
            NextState();

        }
        else
        {
            m_States[m_CurrentStateIndex].UpdateState();
        }
    }

    public override void OnStateAbort()
    {
        Debug.Log("STATE aborts " + m_CurrentStateIndex);
        if (m_CurrentStateIndex < m_States.Count && m_CurrentStateIndex >= 0)
        { 
            m_States[m_CurrentStateIndex].OnStateAbort();
        }
    }

    //these functions will only be called if the stateMachine is part of a stateMachine
    //this relates to the state machine treated as a state that is executed by statemachine
    public override void OnStateEnter()
    {
        m_CurrentStateIndex = -1;
    }

    public override void OnStateExit()
    {
    }




    public sealed override bool IsDone()
    {
        if(m_CurrentStateIndex >= m_States.Count)
        {
            return true;
        }
        return false;
    }

    void NextState()
    {
        bool stateIsBreaking = m_States[m_CurrentStateIndex].m_Break;


        m_States[m_CurrentStateIndex].m_Break = false;

        if (!stateIsBreaking)
        {
            m_States[m_CurrentStateIndex].OnStateExit();
        }
        else
        {
            m_States[m_CurrentStateIndex].OnStateAbort();
        }
        
        //do i need to loop the state?
        bool isLoopingState = m_States[m_CurrentStateIndex].ShouldLoop();

        //if i dont need to loop, go to next state
        if (!isLoopingState || stateIsBreaking)
        {
            m_CurrentStateIndex++;
        }

        //was there a next state? if yes enter state, if not we are done anyway
        if (!this.IsDone()) // is statemachine done?
        {
            m_States[m_CurrentStateIndex].OnStateEnter();
        } //if we are done but the state machine loops itself then reset the statemachine


        //if done, dont do anything, cause isDone will be true
    }


   

}

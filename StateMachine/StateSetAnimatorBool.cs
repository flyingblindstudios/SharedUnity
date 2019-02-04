using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateSetAnimatorBool : StateMachineState
{
    Animator m_Animator;
    string m_Key;
    bool m_Value;

    public StateSetAnimatorBool(Animator _animator, string _key, bool _value)
    {
        m_Animator = _animator;
        m_Key = _key;
        m_Value = _value;
    }

    public override void UpdateState()
    {

    }

    public override void OnStateEnter()
    {
        m_Animator.SetBool(m_Key, m_Value);
    }

    public override void OnStateExit()
    {

    }

    public override bool IsDone()
    {
        return true;
    }
}

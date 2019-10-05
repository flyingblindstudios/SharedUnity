using System;

namespace Shared.AI
{
    public class StateLamdaCode : StateMachineState
    {
        Action m_Lamda;
        public StateLamdaCode(Action _lamda)
        {
            m_Lamda = _lamda;
        }

        public override void OnStateExit()
        {
            m_Lamda?.Invoke();
        }

        public override bool IsDone()
        {
            return true;
        }

    }
}

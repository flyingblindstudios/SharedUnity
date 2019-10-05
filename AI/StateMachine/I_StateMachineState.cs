using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.AI
{
    public interface I_StateMachineState
    {
        void Break(); //the state breaks and interrupts itself
        void UpdateState();
        void OnStateEnter();
        void OnStateExit();
        void OnStateAbort();
        bool IsDone();
        bool ShouldLoop(); //do you want to loop?
        string GetDebugInfo();
        bool IsBreaking();
        void Reset();
    };
}

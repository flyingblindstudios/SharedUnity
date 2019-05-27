using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shared.Event;
namespace Shared.Event
{
    public class SetObjectOnEvent : Shared.Event.GameEventListenerBase
    {
        [SerializeField]
        GameObject m_Object;

        [SerializeField]
        bool m_Toogle = false;

        [SerializeField]
        bool m_ToState = true;

        public override void OnEventRaised(GameEvent _event)
        {
            SetObject();
        }

        public void SetObject()
        {
            m_Object.SetActive(m_ToState);

            if (m_Toogle)
            {
                m_ToState = !m_ToState;
            }
        }
    }
}

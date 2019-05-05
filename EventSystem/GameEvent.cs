using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Event
{ 
    [CreateAssetMenu(menuName = "Shared/Event/GameEvent")]
    public class GameEvent : ScriptableObject
    {
        private List<IEventListener> m_Listeners = new List<IEventListener>();

        public void Raise()
        {
            for (int i = m_Listeners.Count - 1; i >= 0; i--)
            {
                m_Listeners[i].OnEventRaised(this);
            }
        }

        public void RegisterListener(IEventListener _listerner)
        {
            m_Listeners.Add(_listerner);
        }

        public void UnregisterListener(IEventListener _listerner)
        {
            m_Listeners.Remove(_listerner);
        }
    }
}

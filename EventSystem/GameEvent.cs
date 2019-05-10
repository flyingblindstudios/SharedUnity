using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace Shared.Event
{

    /*public interface I_GameEvent
    {
        void Raise();
    }

    public class GameEventUnityEvent : MonoBehaviour, I_GameEvent
    {
        [SerializeField]
        UnityEvent m_UnityEvent;

        public void Raise()
        {
            if (m_UnityEvent != null)
            {
                m_UnityEvent.Invoke();
            }
        }
    }*/

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

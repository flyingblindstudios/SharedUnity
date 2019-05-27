using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Event
{
    public interface IEventListener
    {
        void OnEventRaised(GameEvent _event);
    }

    public abstract class GameEventListenerBase : MonoBehaviour, IEventListener
    {
        public GameEvent Event;

        private void OnEnable()
        {
            if (Event == null)
            {
                return;
            }

            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            if (Event == null)
            {
                return;
            }

            Event.UnregisterListener(this);
        }
        public abstract void OnEventRaised(GameEvent _event);

    }
}

using UnityEngine;
using UnityEngine.Events;

namespace Shared.Event
{
    public interface IEventListener
    {
        void OnEventRaised( GameEvent _event );
    }


    [CreateAssetMenu(menuName = "Shared/Event/GameEventListener")]
    public class GameEventListener : ScriptableObject, IEventListener
    {
        public GameEvent Event;
        public UnityEvent Response;

        private void OnEnable()
        {
            Event.RegisterListener(this);
        }

        private void OnDisable()
        {
            Event.UnregisterListener(this);
        }
        public void OnEventRaised( GameEvent _event )
        {
            Response.Invoke();
        }

    }
}
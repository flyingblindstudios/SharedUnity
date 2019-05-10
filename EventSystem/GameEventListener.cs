using UnityEngine;
using UnityEngine.Events;

namespace Shared.Event
{

    [CreateAssetMenu(menuName = "Shared/Event/GameEventListener")]
    public class GameEventListener : GameEventListenerBase
    {
 
        public UnityEvent Response;

        public override void OnEventRaised( GameEvent _event )
        {
            Response.Invoke();
        }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ECS.Events
{
	using ECS.Systems;
	
	public class EventHandler : Singleton<EventHandler> {

		
		//[GameEventType]
		//public enum EVENTS { ON_COLLISION_ENTER = 0 }
		

		//public delegate EventDelegate( EVENTS _event );


		//private Dictionary<Events, EventDelegate>

		/*void RegisterEventListener( EVENTS _eventType,  EventDelegate _callback)
		{	
			if (_eventType == null)
			{
				throw new ArgumentNullException("eventType");
			}
		
			if (_eventType == null)
			{
				throw new ArgumentNullException("callback");
			}
		
			if (this.listeners.ContainsKey(eventType))
			{
				this.listeners[eventType] += callback;
			}
			else
			{
				this.listeners[eventType] = callback;
			}
		}*/
		
		
		/*void ProcessEvents()
		{


		}*/



	}
}
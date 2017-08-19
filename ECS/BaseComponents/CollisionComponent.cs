using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class CollisionComponent : IComponent {

	
	static List<CollisionEvent> m_EventPool = new List<CollisionEvent>();
	
	public List<CollisionEvent> m_CurrentEvents;

	public bool m_Entered = false;
	public bool m_Stayed = false;
	public bool m_Exited = false;
	protected override void OnAwake()
	{
		m_CurrentEvents =  new List<CollisionEvent>();

	}

	//should change implementation, so it registeres a tick

	protected override void OnLateUpdate()
	{
		m_EventPool.AddRange(m_CurrentEvents);
		m_CurrentEvents.Clear();
		m_Entered = false;
		m_Stayed = false;
		m_Exited = false;
	}
	
	[System.Serializable]
	public class CollisionEvent
	{
		public enum EVENT_TYPE{ ENTER, STAY, EXIT };

		public EVENT_TYPE m_EventType;
		public Collision m_Collision;
		public CollisionComponent m_OtherComponent;
	}

	void OnCollisionEnter(Collision _collision)
    {
		  AddCollisionEvent(_collision, CollisionEvent.EVENT_TYPE.ENTER);
		  m_Entered = true;
	}

	void OnCollisionExit(Collision _collision)
    {
		  AddCollisionEvent(_collision, CollisionEvent.EVENT_TYPE.EXIT);
		  m_Exited = true;
	}

	void OnCollisionStay(Collision _collision)
    {
		  AddCollisionEvent(_collision, CollisionEvent.EVENT_TYPE.STAY);
		  m_Stayed = true;
	}

	void AddCollisionEvent(Collision _collision, CollisionEvent.EVENT_TYPE _type)
	{
		CollisionComponent cc = _collision.collider.gameObject.GetComponent<CollisionComponent>();
		if( cc != null)
		{
			CollisionEvent ce = GetCollisionEvent();
			ce.m_Collision = _collision;
			ce.m_EventType = _type;
			ce.m_OtherComponent = cc;
			m_CurrentEvents.Add(ce);
		}
	}

	static CollisionEvent GetCollisionEvent()
	{
		CollisionEvent e;
		if(m_EventPool.Count > 0)
		{
			e = m_EventPool[0];
			m_EventPool.RemoveAt(0);
		}
		else 
		{
			e = new CollisionEvent();
		}
		return e;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EntityDescriptor : MonoBehaviour {

	public Dictionary <Type, IComponent> m_IComponents;
	GameObject m_GameObject = null;

	Transform m_Transform = null;

	bool m_EntityStarted = false;

	void Awake()
	{	
		m_IComponents = new Dictionary <Type, IComponent>();
		Debug.Log("Entity awake");
	}

	void Start()
	{
		
		//m_IComponents = GetComponents<IComponent>();
		m_GameObject = this.gameObject;
		m_Transform = this.transform;
		EntityManager.GetInstance().RegisterEntity(this);
		m_EntityStarted = true;
		Debug.Log("Entity started and res");
	}

	void AddComponent(Type _componentType)
	{
		if(_componentType is IComponent)
		{
			IComponent ic =  (IComponent) m_GameObject.AddComponent( _componentType );
		RegisterIComponent(ic);

		}
		else
		{
			Debug.Log("component is not an icomponent");

		}
		
		
	}


	//only called if component already added to gameobject
	public void RegisterIComponent(IComponent _component)
	{
		m_IComponents.Add(_component.GetType(),_component);

		if(m_EntityStarted)
		{
			//fire event to system? Or Entity that components changed
		}

	}

	public void DeRegisterIComponent(IComponent _component)
	{
		m_IComponents.Remove(_component.GetType());

		if(m_EntityStarted)
		{
			//fire event to system? Or Entity that components changed
		}
	}

	void OnDestroy()
	{
		EntityManager.GetInstance().DeRegisterEntity(this);

	}

	//public GetIComponents

}

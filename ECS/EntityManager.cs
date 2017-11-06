using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : Singleton<EntityManager> 
{

	//GetEntitiesWithComponents
	public delegate void EntityEvent( EntityDescriptor _entitiy );

	public EntityEvent OnEntityAddedDelegate;
	public EntityEvent OnEntityRemovedDelegate;
	public List<EntityDescriptor> m_Endities;

	void Awake()
	{
		m_Endities = new List<EntityDescriptor>();
	}

	public void RegisterEntity(EntityDescriptor _entitiy)
	{
		m_Endities.Add(_entitiy);
		OnEntityAddedDelegate(_entitiy);
	}

	public void DeRegisterEntity(EntityDescriptor _entitiy)
	{
		m_Endities.Remove(_entitiy);
		OnEntityRemovedDelegate(_entitiy);
	}

}

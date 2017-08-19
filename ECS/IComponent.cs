using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(EntityDescriptor))]
public class IComponent : MonoBehaviour
{
    EntityDescriptor m_EntityDescriptor;
    void Awake()
    {
        OnAwake();
         m_EntityDescriptor = GetComponent<EntityDescriptor>();
        if(m_EntityDescriptor == null)
        {

            return;
        }

        m_EntityDescriptor.RegisterIComponent(this);
    }


    void Start()
    {
        
        
       
    }

    void LateUpdate()
    {
        OnLateUpdate();

    }

    protected virtual void OnAwake(){}
    //should change implementation, so it registeres a tick
	protected virtual void OnLateUpdate(){}

    void OnDestroy()
    {
        m_EntityDescriptor.DeRegisterIComponent(this);
        
    }
}

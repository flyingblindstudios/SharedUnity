using System;
using System.Collections.Generic;
using UnityEngine;
namespace ECS.Systems
{
    
    
    public abstract class ISystem : MonoBehaviour
    {
        
        public List<EntityDescriptor> m_Entities = null;

        
        /*public ISystem()
        {
            
        }*/

        void Awake()
        {
            m_Entities = new List<EntityDescriptor>();
            EntityManager.GetInstance().OnEntityAddedDelegate += OnEntityCreated;
            EntityManager.GetInstance().OnEntityRemovedDelegate += OnEntityDestroyed;
            SystemManager.GetInstance().RegisterSystem(this);
        }
       
       void Update()
       {
           OnUpdate();
       }
      
        public abstract Type[] AcceptedNodes();

        
        void OnEntityCreated(EntityDescriptor _descriptor)
        {
            Debug.Log("OnEntityCreated system");
            if( CheckEntityComponents(_descriptor))
            {
                m_Entities.Add(_descriptor);
                Debug.Log("entity added! ");
            }
        }

        void OnEntityDestroyed(EntityDescriptor _descriptor)
        {
           m_Entities.Remove(_descriptor);
        }

        bool CheckEntityComponents(EntityDescriptor _descriptor)
        {
            Type[] types = AcceptedNodes();
            bool okay = true;
            for(int i = 0; i < types.Length; i++)
            {
                if(!_descriptor.m_IComponents.ContainsKey(types[i]))
                {
                    okay = false;
                    break;
                }

            }
            return okay;
        }

        //we get that event from SystemManager
        public void OnEntityChanged( EntityDescriptor _enity )
        {
            
            bool componentsRight = CheckEntityComponents(_enity);
            if(!componentsRight)
            {
                m_Entities.Remove(_enity);
            }
            else if(componentsRight && !m_Entities.Contains(_enity))
            {
               m_Entities.Add(_enity);
            }

        }
        
        protected abstract void OnUpdate();
    }

}

using System;
using System.Collections.Generic;
using UnityEngine;
namespace ECS.Systems
{
    
    
    //TODO: Make update by behaviour manager, so we can decide to receive update or not!s
    public abstract class ISystem : MonoBehaviour
    {
        
        public List<EntityDescriptor> m_Entities = null;
        public bool m_UseFixedUpdate = false;
        
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
           if(m_Entities.Count == 0 || m_UseFixedUpdate)
           {
               return;
           }


           
           
           OnUpdate();
       }

       void FixedUpdate()
       {
           if(m_Entities.Count == 0 || !m_UseFixedUpdate)
           {
               return;
           }
           OnUpdate();
       }

      
        public abstract Type[] AcceptedNodes();

        
        void OnEntityCreated(EntityDescriptor _descriptor)
        {
            if( CheckEntityComponents(_descriptor))
            {
                m_Entities.Add(_descriptor);
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

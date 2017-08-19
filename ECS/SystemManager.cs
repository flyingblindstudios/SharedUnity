using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ECS.Systems
{
    public class SystemManager : Singleton<SystemManager>
    {
        List<ISystem> m_Systems;

        void Awake()
        {
            m_Systems = new List<ISystem>();
        }
        
       public void RegisterSystem(ISystem _system)
       {
            m_Systems.Add(_system);
            
            
            //inform systems of exisiting entites
            /*int nEnities =  EntityManager.GetInstance().m_Endities.Count;
            for(int i = 0; i < nEnities; i++)
            {
                _system.

            }*/

       }

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface I_Service
{


}
//maybe add a service factory which can instantiate services
public class ServiceLocator : Singleton<ServiceLocator>
{

    class ServiceReadyPair
    {

        public ServiceReady_delegate m_Callback;
        public System.Type m_Type;
        public bool m_KeepUpdated = false;

        public ServiceReadyPair(System.Type _type, ServiceReady_delegate _del, bool _keepUpdated)
        {
            m_Type = _type;
            m_Callback = _del;
            m_KeepUpdated = _keepUpdated;
        }
    }

    public delegate void ServiceReady_delegate(I_Service _service);


    Dictionary<System.Type, I_Service> m_Services = new Dictionary<System.Type, I_Service>();

    List<ServiceReadyPair> m_ServiceReadyQueue = new List<ServiceReadyPair>();


    void RegisterCallback()
    {

        //if already there assign
    }

    public void UnregisterService(I_Service _service)
    {
        m_Services.Remove(_service.GetType());
    }

    public void RegisterServiceAsType(I_Service _service, System.Type _type)
    {


        if (m_Services.ContainsKey(_type))
        {
            Debug.LogError("More then one of the same service tries to register itself");

        }

        Debug.Log("[ServiceLocator] Registerd Service " + _type.Name);

        m_Services[_type] = _service;

        CheckServiceReadyQueue(_type, _service);
    }

    public void RegisterService(I_Service _service)
    {
        RegisterServiceAsType(_service, _service.GetType());
    }

    void CheckServiceReadyQueue(System.Type _type, I_Service _service)
    {
        ServiceReadyPair pair = null;
        for (int i = m_ServiceReadyQueue.Count-1; i >= 0; i--)
        {
            pair = m_ServiceReadyQueue[i];
            if(pair.m_Type == _type)
            {
                if(!pair.m_KeepUpdated)
                {
                    m_ServiceReadyQueue.RemoveAt(i);

                }
                pair.m_Callback(_service);
            }

        }

    }

    //maybe possiblity to provide a callback if no service is registerd yet!
    public void RequestServiceByCallback( System.Type _type, ServiceReady_delegate _callback, bool _keepUpdated = false)
    {
        if (m_Services.ContainsKey(_type))
        {
            _callback(m_Services[_type]);
        }
        else
        {
            m_ServiceReadyQueue.Add(new ServiceReadyPair(_type,_callback,_keepUpdated));
        }
    }

    public I_Service RequestService(System.Type _type)
    {
        return m_Services[_type];
    }

    public T RequestService<T>() where T: I_Service 
    {
        return (T)m_Services[typeof(T)];
    }

}
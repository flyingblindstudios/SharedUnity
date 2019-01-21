using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    List<GameObject> m_Free = new List<GameObject>();
    //List<GameObject> m_Used = new List<GameObject>(); //shoud be faster to not care about used once (just give back)
    GameObject m_Prefab;
    Transform m_ParentObject;

    public ObjectPool(GameObject _prefab)
    {
        m_ParentObject = new GameObject("Objectpool").transform;
        m_Prefab = _prefab;
    }

    public GameObject GetObject(bool active = true)
    {
        if (m_Free.Count > 0)
        {
            GameObject freeObj = m_Free[0];
            freeObj.SetActive(active);
            m_Free.RemoveAt(0);
            return freeObj;
        }

        GameObject newObj = GameObject.Instantiate(m_Prefab, m_ParentObject);
        newObj.SetActive(active);
        PoolableObject poolable = newObj.GetComponent<PoolableObject>();

        if (poolable != null)
        {
            poolable.SetPool(this);
        }


        return newObj;
    }


    public void ReturnObject(PoolableObject _obj)
    {
        ReturnObject(_obj.gameObject);
    }

    public void ReturnObject(GameObject _obj)
    {
        m_Free.Add(_obj);
        _obj.SetActive(false);
    }
}

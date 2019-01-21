using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolableObject : MonoBehaviour {

    ObjectPool m_Pool;

    public void SetPool(ObjectPool _pool)
    {
        m_Pool = _pool;
    }

    public ObjectPool GetPool()
    {
        return m_Pool;
    }
}

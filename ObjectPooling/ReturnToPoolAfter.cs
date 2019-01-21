using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PoolableObject))]
public class ReturnToPoolAfter : MonoBehaviour {

    [SerializeField]
    float m_Time = 2.0f;

    PoolableObject m_Pool;

    WaitForSeconds m_WaitForSeconds;
    Coroutine routine = null;

    void Awake()
    {
        m_Pool = GetComponent<PoolableObject>();

        m_WaitForSeconds = new WaitForSeconds(m_Time);

    }


    void OnEnable()
    { 
        routine = StartCoroutine(ReturnToPool());
    }

    private void OnDisable()
    {
        if (routine != null)
        {
            StopCoroutine(routine);
            routine = null;
        }
    }

    IEnumerator ReturnToPool()
    {
        yield return m_WaitForSeconds;
        ObjectPool pool = m_Pool.GetPool();
        if (pool != null)
        {
            pool.ReturnObject(m_Pool);
        }
    }

}

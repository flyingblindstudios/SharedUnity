using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectAfter : MonoBehaviour {

    [SerializeField]
    float m_Time = 1.0f;
    
    // Use this for initialization
	void Start ()
    {
        Destroy(this.gameObject, m_Time);
	}

}

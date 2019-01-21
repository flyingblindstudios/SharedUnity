using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatePosition : MonoBehaviour {

    [SerializeField]
    float m_Speed = 1.0f;

    //float m_Duration
	
	// Update is called once per frame
	void Update ()
    {
        this.transform.position += this.transform.up * Time.deltaTime * m_Speed;
	}
}

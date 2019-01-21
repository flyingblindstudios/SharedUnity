using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt : MonoBehaviour {
	
	public Transform m_TransfromToLookAt = null;
	
	// Update is called once per frame
	void Update () {
		if(m_TransfromToLookAt != null)
		{
			this.transform.LookAt(m_TransfromToLookAt);
		
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//maybe use a filter system ? define time after touch, touch id etc. to get a value
public class TouchInputManager : Singleton<TouchInputManager> {

	public List<TouchTracker> m_TouchTracker = new List<TouchTracker>(); 

	int m_MaxSamples = 20;

	void Awake () 
	{
		 m_TouchTracker.Add(new TouchTracker(0,m_MaxSamples));
		 m_TouchTracker.Add(new TouchTracker(1,m_MaxSamples));
	}
	

	void Update () 
	{
		for(int i = 0; i < m_TouchTracker.Count;i++)	
		{
			m_TouchTracker[i].Update();
		}
		//sample touch diff!? And current state
	}
}

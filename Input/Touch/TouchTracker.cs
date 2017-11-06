using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//using realtimesincestartup
public class TouchTracker
{
	int m_TouchID;
	public class TouchData
	{
		public float m_Time;
		public Vector2 m_Position; 
	}
	
	
	public List<TouchData> m_TouchData = new List<TouchData>(); 

	public bool m_TouchDidMove = false;
	public bool m_TouchDidBegin = false;

	public float m_TouchBeganTime;

	public Vector2 m_LastPosition;

	int m_NumberOfTouches = 0;

	public TouchTracker( int _id, int _maxSample )
	{
		m_TouchID = _id;
	}

	public void Update () 
	{
		#if !UNITY_EDITOR
		if(Input.touchCount <= m_TouchID)
		{
			return;
		}
		#endif

		if(m_TouchDidBegin  && m_NumberOfTouches != Input.touchCount)
		{
			//Reset
			m_LastPosition = TouchInputWrapper.GetTouchPosition(m_TouchID);
		}
		
		
		if(GetTouchBegan())
		{
			Debug.Log("Touchbegan");
			m_TouchDidMove = false;
			m_TouchDidBegin = true;
			m_TouchBeganTime = Time.realtimeSinceStartup;
			m_LastPosition = TouchInputWrapper.GetTouchPosition(m_TouchID);
		}
		else if(GetTouchEnded())
		{
			Debug.Log("GetTouchEnded");
			m_TouchDidMove = false;
			m_TouchDidBegin = false;
		}
		else if(GetTouchMoved())
		{
//			Debug.Log("GetTouchMoved");
			m_TouchDidMove = true;
			//sample data
			TakeSample();
			m_LastPosition = TouchInputWrapper.GetTouchPosition(m_TouchID);
		}
		else if(GetTouchStationary())
		{
			
		}

		m_NumberOfTouches = Input.touchCount;
	}

	void TakeSample()
	{
		
		TouchData data;
		
		if(m_TouchData.Count < 20)
		{
			data = new TouchData();
		}
		else
		{
			data = m_TouchData[m_TouchData.Count-1];
			m_TouchData.RemoveAt(m_TouchData.Count-1);	
		}

		data.m_Position = TouchInputWrapper.GetTouchPosition(m_TouchID);
		data.m_Time = Time.realtimeSinceStartup;

		m_TouchData.Insert(0,data);
	}

	public float GetTimeSinceTouchBegan()
	{
		if(m_TouchDidBegin)
		{
			return Time.realtimeSinceStartup - m_TouchBeganTime;
		}

		return float.MaxValue;
	}

	public Vector2 GetPosition()
	{
		
		return TouchInputWrapper.GetTouchPosition(m_TouchID);
	}


	public bool GetTouchBegan()
	{
		
		#if UNITY_EDITOR
		
		if(Input.GetMouseButtonDown(m_TouchID))
		{
			return true;
		}
		
		#else
		
		
		if(Input.GetTouch(m_TouchID).phase == TouchPhase.Began)
		{
			return true;
		}
		#endif

		return false;
	}

	public bool GetTouchEnded()
	{
		#if UNITY_EDITOR
		
		if(Input.GetMouseButtonUp(m_TouchID))
		{
			return true;
		}
		
		#else
		if(Input.GetTouch(m_TouchID).phase == TouchPhase.Ended)
		{
			return true;
		}
		#endif

		return false;
	}

	public bool GetTouchMoved()
	{
		#if UNITY_EDITOR
		
		if(m_TouchDidBegin && (((Vector2)Input.mousePosition)-m_LastPosition).magnitude > 0.1f)
		{
			return true;
		}
		
		#else
		if(Input.GetTouch(m_TouchID).phase == TouchPhase.Moved)
		{
			return true;
		}
		#endif

		return false;
	}

	public bool GetTouchStationary()
	{
		#if UNITY_EDITOR
		#else
		
		if(Input.GetTouch(m_TouchID).phase == TouchPhase.Stationary)
		{
			return true;
		}
		#endif

		return false;
	}


}

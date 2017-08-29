
using UnityEngine;

public class TouchInputWrapper 
{
	public static Vector2 GetTouchPosition( int _touch )
	{
		#if UNITY_EDITOR
			return Input.mousePosition;
		#else
			return Input.touches[_touch].position;
		#endif
	}

	public static bool GetTouchDown( int _touch )
	{
		#if UNITY_EDITOR
			return Input.GetMouseButtonDown(_touch);
		#else
			return Input.touches[_touch].phase == TouchPhase.Begin ;
		#endif

	}

	public static bool GetTouch(int _touch)
	{
		#if UNITY_EDITOR
			return Input.GetMouseButton(_touch);
		#else
			return Input.touchCount-1 >= _touch;
		#endif

	}

	public static int GetTouchCount()
	{
		#if UNITY_EDITOR
			return 0;
		#else
			return Input.touchCount;
		#endif

	}
		
	
}

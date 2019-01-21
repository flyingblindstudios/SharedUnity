using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimateScaling : MonoBehaviour
{

    [SerializeField]
    float m_ScaleDefault = 2.0f;

    [SerializeField]
    float m_Range = 1.0f;

    [SerializeField]
    float m_Speed = 1.0f;

    Vector3 newScale;

	// Update is called once per frame
	void Update () {
        float value = m_ScaleDefault + Mathf.Sin(Time.time*m_Speed)*m_Range;
        newScale.x = value;
        newScale.y = value;
        newScale.z = value;
        this.transform.localScale = newScale;
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseScrollInput : MonoBehaviour
{
    [SerializeField]
    Shared.Data.FloatVariable m_ScrollValue;

    private void Update()
    {
        if (!m_ScrollValue)
        {
            return;
        }

        m_ScrollValue.SetValue(UnityEngine.Input.mouseScrollDelta.y);
    }
}

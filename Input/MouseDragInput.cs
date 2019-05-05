using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shared.Input
{
    public class MouseDragInput : MonoBehaviour
    {
        [SerializeField]
        int m_MouseButtonIndex = 1;

        [SerializeField]
        Shared.Data.Vector3Variable m_Output;


        bool m_DragStarted;

        Vector3 m_LastMousePosition = Vector3.zero;

        // Update is called once per frame
        void Update()
        {
            if (UnityEngine.Input.GetMouseButtonDown(m_MouseButtonIndex))
            {
                m_DragStarted = true;
            }
            else if (UnityEngine.Input.GetMouseButtonUp(m_MouseButtonIndex))
            {
                m_DragStarted = false;
                if (m_Output != null)
                {
                    m_Output.SetValue(Vector3.zero);
                }
            }

            if (m_DragStarted)
            {
                if (m_Output != null)
                {
                    m_Output.SetValue(UnityEngine.Input.mousePosition-m_LastMousePosition);
                }
            }


            m_LastMousePosition = UnityEngine.Input.mousePosition;
        }
    }
}

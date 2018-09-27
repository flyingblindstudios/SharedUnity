using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//this class displays the value from Health data field into a textmesh.
public class UI_Healthbar : MonoBehaviour
{



    [SerializeField]
    Health m_HealthValue;

    [SerializeField]
    TextMesh m_TextMesh;

    void Awake()
    {
        m_HealthValue.OnChange += SetHealthUI;
    }

    void SetHealthUI(FloatValue _float)
    {
        m_TextMesh.text = ((int)(m_HealthValue.GetRatio() * 100)).ToString();
        if (m_HealthValue.GetRatio() <= 0.0f)
        {
            m_TextMesh.gameObject.SetActive(false);
        }
       
    }


}

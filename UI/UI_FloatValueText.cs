using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TMPro.TextMeshProUGUI))]
public class UI_FloatValueText : MonoBehaviour
{
    [SerializeField]
    Shared.Data.FloatVariable m_Variable;

    [SerializeField]
    TMPro.TextMeshProUGUI m_Text;

    private void Update()
    {
        m_Text.text = m_Variable.RuntimeValue.ToString();
    }

}

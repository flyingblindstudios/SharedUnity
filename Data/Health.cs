using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : FloatValue
{
    float m_DefaultHealth;

    public void Awake()
    {
        m_DefaultHealth = m_Value;
    }

    public void DealDamage(float _Amount)
    {
        float value = GetValue() - _Amount;
        value = Mathf.Clamp(value, 0, m_DefaultHealth);
        SetValue(value);
    }

    public override void SetValue(float value)
    {
        base.SetValue(Mathf.Clamp(value,0, m_DefaultHealth));
    }

    public bool IsZero()
    {
        if (m_Value == 0)
        {
            return true;
        }

        return false;
    }

    public float GetRatio()
    {
        return m_Value/m_DefaultHealth;
    }

}

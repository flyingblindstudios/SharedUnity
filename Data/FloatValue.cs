using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class just holds an abritery float value, its possible to get a callback when value is changed

public class FloatValue : MonoBehaviour
{
    public delegate void ON_CHANGE(FloatValue _floatValue);

    //onchange callback delegate
    public ON_CHANGE OnChange;

    public string m_FloatName = "";

    public float m_Value;

    public float GetValue()
    {
        return m_Value;
    }

    public virtual void SetValue(float value)
    {
        m_Value = value;

        if (OnChange != null)
        {
            OnChange(this);
        }
    }

}

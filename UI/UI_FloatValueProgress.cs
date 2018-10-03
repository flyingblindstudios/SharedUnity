using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//this class displays the value from Health data field into a progressbar.

public class UI_FloatValueProgress : MonoBehaviour {

    [SerializeField]
    Color32 m_MainColor;

    [SerializeField]
    Color32 m_BackgroundColor;

    [SerializeField]
    Image m_BackgroundImage;

    [SerializeField]
    Image m_MainImage;


    [SerializeField]
    Health m_Value;
    // Use this for initialization
    void Start () {
        m_Value.OnChange += OnChange;
        m_MainImage.color = m_MainColor;
        m_BackgroundImage.color = m_BackgroundColor;
        OnChange(m_Value);
    }

    void OnDestroy()
    {
        m_Value.OnChange -= OnChange;

    }

    // Update is called once per frame
    void OnChange(FloatValue _value)
    {
        m_MainImage.GetComponent<RectTransform>().sizeDelta = new Vector2(m_Value.GetRatio() * 100,20);

    }


    private void OnValidate()
    {
        if (m_MainImage == null || m_BackgroundImage == null)
        {
            return;
        }

        m_MainImage.color = m_MainColor;
        m_BackgroundImage.color = m_BackgroundColor;
    }
}

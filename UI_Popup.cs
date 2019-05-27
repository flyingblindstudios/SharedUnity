using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public interface Popup : I_Service
{
    void ShowPopup(string _title, string _message, Action _onOkay, Action _onCancle);
};

public class UI_Popup : MonoBehaviour, Popup
{
    Action m_OnOkay;
    Action m_OnCancle;

    [SerializeField]
    TextMeshProUGUI m_TitleText;

    [SerializeField]
    TextMeshProUGUI m_MessageText;

    [SerializeField]
    GameObject m_PopupContent;


    private void Awake()
    {
        ServiceLocator.GetInstance().RegisterServiceAsType(this, typeof(Popup));
        m_PopupContent.SetActive(false);
    }

    void OnDestroy()
    {
        ServiceLocator.GetInstance().UnregisterService(this);
    }


    public void ShowPopup(string _title, string _message, Action _onOkay, Action _onCancle )
    {
        m_TitleText.text = _title;
        m_MessageText.text = _message;

        m_OnOkay = _onOkay;
        m_OnCancle = _onCancle;

        m_PopupContent.SetActive(true);

        this.transform.SetAsLastSibling();
    }


    public void OnCancle()
    {
        m_OnCancle.Invoke();
        m_PopupContent.SetActive(false);
    }

    public void OnOkay()
    {
        m_OnOkay.Invoke();
        m_PopupContent.SetActive(false);
    }

}

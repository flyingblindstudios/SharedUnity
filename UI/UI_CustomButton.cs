using UnityEngine.EventSystems;
using UnityEngine;

namespace Shared.UI
{ 
    public class UI_CustomButton : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        Shared.Event.GameEvent m_OnClick;

        public void OnPointerClick(PointerEventData pointerEventData)
        {
            if (m_OnClick != null)
            {
                m_OnClick.Raise();
            }
        }
    }
}

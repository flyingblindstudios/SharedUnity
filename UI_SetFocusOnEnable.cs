using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_SetFocusOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        this.transform.SetAsLastSibling();
    }
}

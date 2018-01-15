using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropHandler : MonoBehaviour,IDropHandler {

    #region IDropHandler implementation
    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerDrag);
    }
    #endregion
}

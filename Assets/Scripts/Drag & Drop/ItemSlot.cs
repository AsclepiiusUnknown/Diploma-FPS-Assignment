using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject currentObject;

    public void OnDrop(PointerEventData _eventData)
    {
        if (_eventData.pointerDrag != null)
        {
            _eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            currentObject = _eventData.pointerDrag;
            currentObject.GetComponent<DragDrop>().currentSlot = this;
        }
    }

    public void ReleaseObject()
    {
        currentObject = null;
    }
}

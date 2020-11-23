using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public GameObject currentLoadout = null;
    public bool isSelection = false;

    public void OnDrop(PointerEventData _eventData)
    {
        if (_eventData.pointerDrag != null)
        {
            if (currentLoadout != null)
            {
                currentLoadout.GetComponent<RectTransform>().anchoredPosition = _eventData.pointerDrag.GetComponent<DragDrop>().currentItemSlot.GetComponent<RectTransform>().anchoredPosition;
                currentLoadout.GetComponent<DragDrop>().currentItemSlot = _eventData.pointerDrag.GetComponent<DragDrop>().currentItemSlot.gameObject;
            }
            _eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            _eventData.pointerDrag.GetComponent<DragDrop>().currentItemSlot = gameObject;
            _eventData.pointerDrag.GetComponent<CanvasGroup>().blocksRaycasts = false;
            currentLoadout = _eventData.pointerDrag.gameObject;

            if (_eventData.pointerDrag.GetComponent<LoadoutHandler>() != null && isSelection)
            {
                LoadoutTypes tempLoadout = _eventData.pointerDrag.GetComponent<LoadoutHandler>().objectLoadout;
                GameManager.loadout = tempLoadout;
                print("loadout is now " + tempLoadout);
            }
        }
    }
}

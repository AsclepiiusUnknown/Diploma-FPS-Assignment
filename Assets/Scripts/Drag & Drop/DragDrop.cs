using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform), typeof(CanvasGroup))]
public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    RectTransform rectTransform;
    public Canvas canvas;
    CanvasGroup canvasGroup;
    TestItemData itemData;
    public ItemSlot currentSlot;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (GetComponent<TestItemData>() != null)
        {
            itemData = GetComponent<TestItemData>();
        }
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        Debug.Log("OnPointerDown");
    }

    public void OnBeginDrag(PointerEventData _eventData)
    {
        Debug.Log("Begin Drag");
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData _eventData)
    {
        Debug.Log("End Drag");
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnDrag(PointerEventData _eventData)
    {
        Debug.Log("On Drag");
        rectTransform.anchoredPosition += _eventData.delta / canvas.scaleFactor;
    }

    public void OnDrop(PointerEventData _eventData)
    {

    }
}

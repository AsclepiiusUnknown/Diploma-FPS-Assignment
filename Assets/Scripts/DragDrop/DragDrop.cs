using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Canvas canvas;
    RectTransform rectTransform;
    CanvasGroup canvasGroup;
    public GameObject currentItemSlot;


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData _eventData)
    {
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = .65f;
    }

    public void OnDrag(PointerEventData _eventData)
    {
        rectTransform.anchoredPosition += _eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData _eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1;
    }

    public void OnPointerDown(PointerEventData _eventData)
    {
        print("Pointer Down");
    }
}

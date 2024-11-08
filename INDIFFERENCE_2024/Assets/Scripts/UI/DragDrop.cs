using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    public static GameObject dragIcon;


    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject duplicate = Instantiate(gameObject);
        dragIcon = duplicate;
        RectTransform iconRT = gameObject.GetComponent<RectTransform>();

        RectTransform rt = dragIcon.GetComponent<RectTransform>();
        rt.sizeDelta = new Vector2(iconRT.sizeDelta.x, iconRT.sizeDelta.y);

        GetComponent<CanvasGroup>().blocksRaycasts = false;
        Transform canvas = GameObject.FindGameObjectWithTag("UI Canvas").transform;
        dragIcon.transform.SetParent(canvas);
        dragIcon.GetComponent<CanvasGroup>().blocksRaycasts = false;     
    }

    public void OnDrag(PointerEventData eventData)
    {
        dragIcon.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(dragIcon);
        dragIcon = null;
    }
}
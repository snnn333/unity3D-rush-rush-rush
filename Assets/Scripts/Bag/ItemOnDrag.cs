using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemOnDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform originalParent;
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.parent.parent);
        transform.position = eventData.position;

        GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
        //Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.pointerCurrentRaycast.gameObject.name == "itemImage")
        {
            // Swap 2 item
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform.parent.parent);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.parent.parent.position;
            
            eventData.pointerCurrentRaycast.gameObject.transform.parent.transform.SetParent(originalParent);
            eventData.pointerCurrentRaycast.gameObject.transform.parent.position = originalParent.position;
        }
        else if (eventData.pointerCurrentRaycast.gameObject.name == "gridItem(Clone)")
        {
            // Move to empty grid
            transform.SetParent(eventData.pointerCurrentRaycast.gameObject.transform);
            transform.position = eventData.pointerCurrentRaycast.gameObject.transform.position;
        }
        else
        {
            // Get back to original position
            transform.SetParent(originalParent);
            transform.position = originalParent.position;
        }
        
        GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
}

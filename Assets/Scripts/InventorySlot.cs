using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    void IDropHandler.OnDrop(PointerEventData eventData)
    {
        GameObject dropped = eventData.pointerDrag;
        DraggableItem draggableItem = dropped.GetComponent<DraggableItem>();
        if (!draggableItem)
            return;
        if (transform.childCount > 0)
            return;

        draggableItem.SetParentAfterDrag(transform);
    }
}

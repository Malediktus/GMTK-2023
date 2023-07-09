using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public void AddItem(GameObject item)
    {
        DraggableItem draggableItem = item.GetComponent<DraggableItem>();
        if (!draggableItem)
        {
            item.AddComponent<DraggableItem>();
        }

        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                continue;
            }

            item.transform.SetParent(child);
            break;
        }
    }

    public bool Full()
    {
        return false;
        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                continue;
            }

            return false;
        }
        return true;
    }

    public void RemoveItem(GameObject item)
    {
        DraggableItem draggableItem = item.GetComponent<DraggableItem>();
        if (draggableItem)
        {
            Destroy(draggableItem);
        }
    }
}

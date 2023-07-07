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
            return;
        }

        foreach (Transform child in transform)
        {
            if (child.childCount > 0)
            {
                continue;
            }

            item.transform.parent = child;
            break;
        }
    }
}

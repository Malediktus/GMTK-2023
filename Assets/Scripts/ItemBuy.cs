using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBuy : MonoBehaviour
{
    [SerializeField] private Transform itemSlot;
    [SerializeField] private TextMeshProUGUI buyText;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private GameObject draggableItemPrefab;

    private void OnEnable()
    {
        ShopItem shopItem = NPCManager.Instance.GetCurrentVisitor().inventory[0];
        GameObject draggableItemInstance = Instantiate(draggableItemPrefab, itemSlot);

        //I dont know how buying works so I dont know how to convert this to the scriptable system - Eyad

        //ShopItem component = draggableItemInstance.AddComponent<ShopItem>(); 
        //component.itemType = shopItem.itemType;
        //component.image = shopItem.image;
        //component.level = shopItem.level;
        //component.quality = shopItem.quality;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBuy : MonoBehaviour
{
    [SerializeField] private Transform itemSlot;
    [SerializeField] private TMP_Text buyText;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private GameObject draggableItemPrefab;
    [SerializeField] private Inventory inventory;
    [SerializeField] private float cost;

    private void OnEnable()
    {
        ShopItem shopItem = NPCManager.Instance.GetCurrentVisitor().inventory[0];
        GameObject draggableItemInstance = Instantiate(draggableItemPrefab, itemSlot);
        DraggableItem draggableItemComponent = draggableItemInstance.GetComponent<DraggableItem>();
        cost = shopItem.EvaluateCost(0) + Random.Range(0, NPCManager.Instance.GetCurrentVisitor().trust);
        buyText.text = cost + " coins";

        draggableItemComponent.shopItem = shopItem;
        draggableItemComponent.draggable = false;
    }

    private void OnDisable()
    {
        if (itemSlot.childCount <= 0)
            return;

        Destroy(itemSlot.GetChild(0));
    }

    public void BuyItem()
    {
        if (itemSlot.childCount <= 0)
        {
            return;
        }
        DraggableItem item = itemSlot.GetChild(0).GetComponent<DraggableItem>();
        ShopItem shopItem = item.GetItem();

        var visitor = NPCManager.Instance.GetCurrentVisitor();
        Debug.Log($"{visitor == null} || {inventory.Full()} || {cost > moneyManager.GetMoney()}");
        if (visitor == null || inventory.Full() || cost > moneyManager.GetMoney())
        {
            return;
        }

        visitor.inventory.Remove(shopItem);
        visitor.money += cost;

        moneyManager.SubMoney(cost);
        item.draggable = true;
        inventory.AddItem(item.gameObject);
    }
}

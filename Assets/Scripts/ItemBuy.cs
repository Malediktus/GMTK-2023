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

    private void OnEnable()
    {
        ShopItem shopItem = NPCManager.Instance.GetCurrentVisitor().inventory[0];
        GameObject draggableItemInstance = Instantiate(draggableItemPrefab, itemSlot);
        DraggableItem draggableItemComponent = draggableItemInstance.GetComponent<DraggableItem>();
        buyText.text = shopItem.EvaluateCost(0) + " coins";

        draggableItemComponent.shopItem = shopItem;
        draggableItemComponent.draggable = false;
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
        Debug.Log($"{visitor == null} || {inventory.Full()} || {shopItem.EvaluateCost(0) > moneyManager.GetMoney()}");
        if (visitor == null || inventory.Full() || shopItem.EvaluateCost(0) > moneyManager.GetMoney())
        {
            return;
        }

        visitor.inventory.Remove(shopItem);
        visitor.money += shopItem.EvaluateCost(0);

        moneyManager.SubMoney(shopItem.EvaluateCost(0));
        item.draggable = true;
        inventory.AddItem(item.gameObject);
    }
}

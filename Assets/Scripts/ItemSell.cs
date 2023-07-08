using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemSell : MonoBehaviour
{
    [SerializeField] private Transform sellSlot;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private MoneyManager moneyManager;

    private void Update()
    {
        if (sellSlot.childCount > 0)
        {
            ShopItem item = sellSlot.GetChild(0).GetComponent<ShopItem>();

            sellText.text = item.EvaluateCost(0).ToString() + " coins";
        }
        else
        {
            sellText.text = "Add an item";
        }
    }

    public void SellItem()
    {
        if (sellSlot.childCount > 0)
        {
            ShopItem item = sellSlot.GetChild(0).GetComponent<ShopItem>();

            NPCManager.Instance.GetCurrentVisitor().inventory.Add(item);

            moneyManager.AddMoney(item.EvaluateCost(0));
            Destroy(item.gameObject);
        }
    }
}

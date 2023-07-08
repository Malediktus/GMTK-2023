using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSell : MonoBehaviour
{
    [SerializeField] private Transform sellSlot;
    [SerializeField] private Slider moneySlider;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private MoneyManager moneyManager;

    private void Update()
    {
        if (sellSlot.childCount > 0)
        {
            ShopItem item = sellSlot.GetChild(0).GetComponent<DraggableItem>().GetItem();

            sellText.text = (item.EvaluateCost(0) + Mathf.Round(moneySlider.value * 10.0f) * 0.1f).ToString() + " coins";
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
            DraggableItem item = sellSlot.GetChild(0).GetComponent<DraggableItem>();
            ShopItem ShopItem = item.GetItem();

            var visitor = NPCManager.Instance.GetCurrentVisitor();
            float additional = Mathf.Round(moneySlider.value * 10.0f) * 0.1f;
            if (visitor == null || !visitor.EvaluateTrade(ShopItem, additional))
            {
                return;
            }

            visitor.inventory.Add(ShopItem);
            visitor.money -= ShopItem.EvaluateCost(additional);

            moneyManager.AddMoney(ShopItem.EvaluateCost(additional));
            Destroy(item.gameObject);
        }
    }
}

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
            ShopItem item = sellSlot.GetChild(0).GetComponent<ShopItem>();

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
            ShopItem item = sellSlot.GetChild(0).GetComponent<ShopItem>();
            var visitor = NPCManager.Instance.GetCurrentVisitor();
            float additional = Mathf.Round(moneySlider.value * 10.0f) * 0.1f;
            if (visitor == null || !visitor.EvaluateTrade(item, additional))
            {
                return;
            }

            visitor.inventory.Add(item);
            visitor.money -= item.EvaluateCost(additional);

            moneyManager.AddMoney(item.EvaluateCost(additional));
            Destroy(item.gameObject);
        }
    }
}

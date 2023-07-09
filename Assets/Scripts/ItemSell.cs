using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSell : MonoBehaviour
{
    public static List<string> refusals = new List<string>{"That is way too high!", "Are you trying to rob me?!?!", "I am sorry, this is above my budget"};
    [SerializeField] private Transform sellSlot;
    [SerializeField] private Slider moneySlider;
    [SerializeField] private TextMeshProUGUI sellText;
    [SerializeField] private MoneyManager moneyManager;
    [SerializeField] private Inventory inventory;
    [SerializeField] AudioClip[] sellingClips;
    [SerializeField] AudioSource audioSource;
    private int disatisfaction;

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

    private void OnDisable()
    {
        if (sellSlot.childCount <= 0)
            return;
        DraggableItem item = sellSlot.GetChild(0).GetComponent<DraggableItem>();
        if (item)
            inventory.AddItem(item.gameObject);
    }

    public void SellItem()
    {
        if (sellSlot.childCount > 0)
        {
            DraggableItem item = sellSlot.GetChild(0).GetComponent<DraggableItem>();
            ShopItem ShopItem = item.GetItem();

            switch (ShopItem.itemType)
            {
                case ShopItem.ShopItemType.Armor:
                    audioSource.clip = sellingClips[0];
                    break;
                case ShopItem.ShopItemType.Item:
                    audioSource.clip = sellingClips[1];
                    break;
                case ShopItem.ShopItemType.Potion:
                    audioSource.clip = sellingClips[2];
                    break;
                case ShopItem.ShopItemType.TresureMap:
                    audioSource.clip = sellingClips[3];
                    break;
                case ShopItem.ShopItemType.Weapon:
                    audioSource.clip = sellingClips[4];
                    break;
            }

            audioSource.Play();

            var visitor = NPCManager.Instance.GetCurrentVisitor();
            float additional = Mathf.Round(moneySlider.value * 10.0f) * 0.1f;
            
            if (visitor == null)
            {
                return;
            }

            if (!visitor.EvaluateTrade(ShopItem, additional))
            {
                Dialogue.Instance.Tell(refusals[Random.Range(0, refusals.Count - 1)]);
                disatisfaction += 1;

                if (disatisfaction >= 2)
                {
                    disatisfaction = 0;
                    NPCManager.Instance.OnEndDialog();
                }

                return;
            }

            visitor.inventory.Add(ShopItem);
            visitor.money -= ShopItem.EvaluateCost(additional);
            visitor.trust -= Mathf.RoundToInt(moneySlider.value) - Random.Range(10, 60);

            moneyManager.AddMoney(ShopItem.EvaluateCost(additional));
            Destroy(item.gameObject);
            moneySlider.value = 0;
            Dialogue.Instance.Tell(NPCManager.sellDialog[UnityEngine.Random.Range(0, NPCManager.sellDialog.Count - 1)]);
        }
    }
}

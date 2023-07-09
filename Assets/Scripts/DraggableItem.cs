using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using static UnityEditor.Progress;

public class DraggableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDrag;
    private Image image;
    [SerializeField] public ShopItem shopItem;
    private TMP_Text tooltipText;
    [SerializeField] public bool draggable = true;
    private Sprite armor;
    private Sprite item;
    private Sprite potion;
    private Sprite tresureMap;
    private Sprite weapon;

    private void Start()
    {
        armor = NPCManager.Instance.armor;
        item = NPCManager.Instance.item;
        potion = NPCManager.Instance.potion;
        tresureMap = NPCManager.Instance.tresureMap;
        weapon = NPCManager.Instance.weapon;

        tooltipText = NPCManager.Instance.tooltipText;
        image = GetComponent<Image>();

        image.sprite = shopItem.GetImage();
        switch (shopItem.itemType)
        {
            case ShopItem.ShopItemType.Armor:
                image.sprite = armor;
                break;
            case ShopItem.ShopItemType.Item:
                image.sprite = item;
                break;
            case ShopItem.ShopItemType.Potion:
                image.sprite = potion;
                break;
            case ShopItem.ShopItemType.TresureMap:
                image.sprite = tresureMap;
                break;
            case ShopItem.ShopItemType.Weapon:
                image.sprite = weapon;
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!draggable)
            return;
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!draggable)
            return;
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!draggable)
            return;
        transform.SetParent(parentAfterDrag);
        image.raycastTarget = true;
    }

    public void SetParentAfterDrag(Transform newParentAfterDrag)
    {
        parentAfterDrag = newParentAfterDrag;
    }

    public ShopItem GetItem()
    {
        return shopItem;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.text = $"Level: {shopItem.level}\nQuality: {shopItem.quality}";
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltipText.text = "No item\nselected";
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DraggableItem : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform parentAfterDrag;
    private Image image;
    [SerializeField] public ShopItem shopItem;
    [SerializeField] private GameObject tooltip;
    [SerializeField] private TMP_Text tooltipText;

    private void Start()
    {
        image = GetComponent<Image>();

        image.sprite = shopItem.GetImage();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        image.raycastTarget = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
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
        tooltip.SetActive(true);
        tooltipText.text = $"Level: {shopItem.level}\nQuality: {shopItem.quality}";
        tooltip.transform.position = new Vector2(eventData.position.x - 58, eventData.position.y - 17);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        tooltip.SetActive(false);
    }
}

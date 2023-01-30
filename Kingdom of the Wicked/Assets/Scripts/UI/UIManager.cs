using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Vector2 cardSize_small = new Vector2(80, 120),
        cardSize_big = new Vector2(120, 160);
    [SerializeField] private float slotCardMargin = 0, inventoryCardMargin = 20;
    [SerializeField] private int inventoryRowCardsCapacity = 10;
    [SerializeField] private Vector2 dragRangeMin = Vector2.zero, dragRangeMax = new (1920, 1080);
    public Vector2 DragRangeMin => dragRangeMin;
    public Vector2 DragRangeMax => dragRangeMax;

    const string k_cardBackground = "CardBackground";
    const string k_cardName = "CardName";

    public void StyleCard(IStorage.StorageNames storageName, VisualElement cardVE, Card card)
    {
        var cardBackgroung = cardVE.Q(k_cardBackground);
        var cardName = cardVE.Q<Label>(k_cardName);
        cardName.text = card.CardName;
        SetSize(cardBackgroung, storageName);
        switch (storageName)
        {
            case IStorage.StorageNames.weaponSlot:
            case IStorage.StorageNames.armorSlot:
            case IStorage.StorageNames.otherSlot:
                {
                    cardBackgroung.style.marginBottom = slotCardMargin;
                    cardBackgroung.style.marginTop = slotCardMargin;
                    cardBackgroung.style.marginLeft = slotCardMargin;
                    cardBackgroung.style.marginRight = slotCardMargin;
                    break;
                }
            case IStorage.StorageNames.inventory:
                {
                    cardBackgroung.style.marginBottom = inventoryCardMargin;
                    cardBackgroung.style.marginTop = inventoryCardMargin;
                    cardBackgroung.style.marginLeft = inventoryCardMargin;
                    cardBackgroung.style.marginRight = inventoryCardMargin;
                    break;
                }
        }
    }

    public void SetSize(VisualElement cardVE, IStorage.StorageNames storageName)
    {
        var size = RequiredCardSize(storageName);
        cardVE.style.width = size.x;
        cardVE.style.height = size.y;
    }

    private Vector2 RequiredCardSize(IStorage.StorageNames storageName)
    {
        switch (storageName)
        {
            case IStorage.StorageNames.weaponSlot:
            case IStorage.StorageNames.armorSlot:
            case IStorage.StorageNames.otherSlot:
                {
                    return cardSize_small;
                }
            default:
                {
                    return cardSize_big;
                }
        }
    }

    public Vector2 GetCardSizeOffset(IStorage.StorageNames storageName)
    {
        var offset = RequiredCardSize(storageName);
        switch (storageName)
        {
            case IStorage.StorageNames.weaponSlot:
            case IStorage.StorageNames.armorSlot:
            case IStorage.StorageNames.otherSlot:
                {
                    offset.x += slotCardMargin;
                    offset.y += slotCardMargin;
                    break;
                }
            case IStorage.StorageNames.inventory:
                {
                    offset.x += inventoryCardMargin;
                    offset.y += inventoryCardMargin;
                    break;
                }
        }
        return offset;
    }
}
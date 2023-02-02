using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class UIManager : Singleton<UIManager>
{
    [SerializeField]
    private Vector2 cardSize_small = new (80, 120),
        cardSize_big = new (120, 160);
    [SerializeField] private float slotCardMargin = 0, inventoryCardMargin = 20;
    [SerializeField] private Vector2 dragRangeMin = new(20, 20), dragRangeMax = new (1900, 1060);
    public Vector2 DragRangeMin => dragRangeMin;
    public Vector2 DragRangeMax => dragRangeMax;
    public Vector2 CardToDragSizeOffset => cardSize_big;

    const string k_cardBackground = "CardBackground";
    const string k_cardName = "CardName";
    const string k_cardDescription = "CardDescription";
    const string k_cardStatBonuses = "CardStatBonuses";

    public void StyleCard(IStorage.StorageNames storageName, VisualElement cardVE, Card card)
    {
        SetText(cardVE, card);
        var cardBackgroung = cardVE.Q(k_cardBackground);
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

    public void StyleCardToDrag(VisualElement cardVE, Card card)
    {
        SetText(cardVE, card);
        var cardBackgroung = cardVE.Q(k_cardBackground);
        SetSize(cardBackgroung, IStorage.StorageNames.inventory);
    }

    private void SetText(VisualElement cardVE, Card card)
    {
        var cardName = cardVE.Q<Label>(k_cardName);
        var cardDescription = cardVE.Q<Label>(k_cardDescription);
        var cardStatBonuses = cardVE.Q<Label>(k_cardStatBonuses);
        cardName.text = card.CardName;
        cardDescription.text = card.Description;
        var statBonuses = "";
        foreach (StatBonus bonus in card.StatBonuses)
        {
            statBonuses += GameDatabase.Instance.StatsDescription[bonus.StatTypeId].name + ": "
                + bonus.Value + "\n";
        }
        cardStatBonuses.text = statBonuses;
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
}

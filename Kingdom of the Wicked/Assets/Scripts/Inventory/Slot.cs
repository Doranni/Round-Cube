using System;
using System.Collections.Generic;

public class Slot : IStorage
{
    public IStorage.StorageNames StorageName { get; private set; }
    public Card.CardsType CardTypes { get; private set; }
    public bool AffectsStats => true;
    public List<Card> Cards { get; private set; }
    public bool CanBeEmpty { get; private set; }

    public event Action CardsChanged;

    public Slot(IStorage.StorageNames name, Card.CardsType cardsType)
    {
        StorageName = name;
        CardTypes = cardsType;
        Cards = new(1);
        if (StorageName == IStorage.StorageNames.WeaponSlot)
        {
            CanBeEmpty = false;
        }
        else
        {
            CanBeEmpty = true;
        }
    }

    public (bool, Card) AddCard(Card card, bool compareCardTypesFlags = true)
    {
        if (compareCardTypesFlags && !GameManager.Instance.ComperaCardTypesFlags(card.CardType, CardTypes))
        {
            return (false, null);
        }
        if (Cards.Count == 0)
        {
            Cards.Add(card);
            CardsChanged?.Invoke();
            return (true, null);
        }
        var releasedCard = Cards[0];
        Cards[0] = card;
        CardsChanged?.Invoke();
        return (true, releasedCard);
    }

    public bool RemoveCard(Card card)
    {
        if (CanBeEmpty)
        {
            if (Cards[0].InstanceId == card.InstanceId)
            {
                Cards.Clear();
                CardsChanged?.Invoke();
                return true;
            }
        }
        return false;
    }
}

public class SlotsHolder
{
    public enum SlotsHolderNames
    { 
        Armor,
        Other
    }

    public SlotsHolderNames SlotsHolderName { get; private set; }
    public List<Slot> Slots { get; private set; }

    public SlotsHolder(SlotsHolderNames slotName, List<Slot> slots)
    {
        SlotsHolderName = slotName;
        Slots = slots;
    }
}

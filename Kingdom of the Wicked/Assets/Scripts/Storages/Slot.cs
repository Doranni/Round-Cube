using System;
using System.Collections.Generic;

public class Slot : IStorage
{
    private Character character;
    public IStorage.StorageNames StorageName { get; private set; }
    public Card.CardsType CardTypes { get; private set; }
    public List<Card> Cards { get; private set; }
    public bool CanBeEmpty { get; private set; }

    public event Action CardsChanged;

    public Slot(IStorage.StorageNames name, Card.CardsType cardsType, Character character)
    {
        this.character = character;
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

    public (bool, Card) AddCard(Card card)
    {
        if (!Card.ComperaCardTypesFlags(card.CardType, CardTypes))
        {
            return (false, null);
        }
        if (Cards.Count == 0)
        {
            Cards.Add(card);
            card.SetCardOwner(character, StorageName);
            if (StorageName == IStorage.StorageNames.ArmorSlot)
            {
                character.Stats.SetArmor((ArmorCard)card);
            }
            else if (StorageName == IStorage.StorageNames.ShieldSlot)
            {
                character.Stats.SetShield((ShieldCard)card);
            }
            if (card is ICardAddStatBonuses)
            {
                foreach (StatBonus bonus in ((ICardAddStatBonuses)card).StatBonuses)
                {
                    character.Stats.AddBonus(bonus);
                }
            }
            CardsChanged?.Invoke();
            return (true, null);
        }
        var releasedCard = Cards[0];
        Cards[0] = card;
        card.SetCardOwner(character, StorageName);
        if (StorageName == IStorage.StorageNames.ArmorSlot)
        {
            character.Stats.SetArmor((ArmorCard)card);
        }
        else if (StorageName == IStorage.StorageNames.ShieldSlot)
        {
            character.Stats.SetShield((ShieldCard)card);
        }
        if (card is ICardAddStatBonuses)
        {
            foreach (StatBonus bonus in ((ICardAddStatBonuses)card).StatBonuses)
            {
                character.Stats.AddBonus(bonus);
            }
        }
        CardsChanged?.Invoke();
        return (true, releasedCard);
    }

    public bool RemoveCard(Card card, bool forceRemove = false)
    {
        if (CanBeEmpty || forceRemove)
        {
            if (Cards.Count == 1 || Cards[0].InstanceId == card.InstanceId)
            {
                if (StorageName == IStorage.StorageNames.ArmorSlot)
                {
                    character.Stats.SetArmor(null);
                }
                else if (StorageName == IStorage.StorageNames.ShieldSlot)
                {
                    character.Stats.SetShield(null);
                }
                if (card is ICardAddStatBonuses)
                {
                    foreach (StatBonus bonus in ((ICardAddStatBonuses)card).StatBonuses)
                    {
                        character.Stats.RemoveBonus(bonus);
                    }
                }
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
        Defense,
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

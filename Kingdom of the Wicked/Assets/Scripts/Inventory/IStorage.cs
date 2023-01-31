using System;
using System.Collections.Generic;

public interface IStorage
{
    [Flags]
    public enum AvailableCardsTypes
    {
        weapon = 1,
        armor = 2,
        other = 4
    }
    public enum StorageNames
    {
        weaponSlot,
        armorSlot,
        otherSlot,
        inventory,
        storage
    }
    public StorageNames StorageName { get; }
    public AvailableCardsTypes CardsTypes { get; }
    public bool AffectsStats { get; }
    public int Capacity { get; }
    public List<Card> Cards { get; }

    public Card AddCard(Card card);
    public bool RemoveCard(Card card);
}

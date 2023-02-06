using System;
using System.Collections.Generic;

public interface IStorage
{
    public enum StorageNames
    {
        weaponSlot,
        armorSlot,
        shieldSlot,
        otherSlot,
        inventory,
        storage
    }
    public StorageNames StorageName { get; }
    public Card.CardsType CardsTypes { get; }
    public bool AffectsStats { get; }
    public int Capacity { get; }
    public List<Card> Cards { get; }

    public Card AddCard(Card card);
    public bool RemoveCard(Card card);
}

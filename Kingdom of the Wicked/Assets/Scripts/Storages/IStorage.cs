using System;
using System.Collections.Generic;

public interface IStorage
{
    public enum StorageNames
    {
        WeaponSlot,
        ArmorSlot,
        ShieldSlot,
        OtherSlot1,
        OtherSlot2,
        OtherSlot3,
        Inventory,
        Reward
    }
    public StorageNames StorageName { get; }
    public Card.CardsType CardTypes { get; }
    public List<Card> Cards { get; }
    public bool IsFull => !(Cards.Count < Cards.Capacity);
    public (bool success, Card releasedCard) AddCard(Card card);
    public bool RemoveCard(Card card, bool forceRemove = false);
    public event Action CardsChanged;
}

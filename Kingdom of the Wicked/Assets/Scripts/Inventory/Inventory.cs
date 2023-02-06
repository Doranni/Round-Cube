using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : IStorage
{
    public IStorage.StorageNames StorageName { get; private set; }
    public Card.CardsType CardsTypes { get; private set; }
    public bool AffectsStats => false;
    public int Capacity => -1;
    public List<Card> Cards { get; private set; }

    public Inventory()
    {
        StorageName = IStorage.StorageNames.inventory;
        CardsTypes = Card.CardsType.Weapon | Card.CardsType.Armor | Card.CardsType.Shield | Card.CardsType.Other;
        Cards = new();
    }

    public Card AddCard(Card card)
    {
        Cards.Add(card);
        return null;
    }

    public bool RemoveCard(Card card)
    {
        var cardToRemove = Cards.Find(x => x.Id == card.Id);
        if (cardToRemove == null)
        {
            return false;
        }
        Cards.Remove(cardToRemove);
        return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : IStorage
{
    public IStorage.StorageNames StorageName { get; private set; }
    public Card.CardsType CardTypes { get; private set; }
    public bool AffectsStats => false;
    public List<Card> Cards { get; private set; }

    public event Action CardsChanged;

    public Chest()
    {
        StorageName = IStorage.StorageNames.Chest;
        CardTypes = Card.CardsType.Weapon | Card.CardsType.Armor | Card.CardsType.Shield | Card.CardsType.Magic
            | Card.CardsType.Potion | Card.CardsType.Artifact;
        Cards = new();
    }

    public (bool success, Card releasedCard) AddCard(Card card, bool compareCardTypesFlags = true)
    {
        Cards.Add(card);
        CardsChanged?.Invoke();
        return (true, null);
    }

    public bool RemoveCard(Card card)
    {
        var cardToRemove = Cards.Find(x => x.NameId == card.NameId);
        if (cardToRemove == null)
        {
            return false;
        }
        Cards.Remove(cardToRemove);
        CardsChanged?.Invoke();
        return true;
    }
}

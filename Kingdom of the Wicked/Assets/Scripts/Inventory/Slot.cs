using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot : IStorage
{
    public IStorage.StorageNames StorageName { get; private set; }
    public IStorage.AvailableCardsTypes CardsTypes { get; private set; }
    public bool AffectsStats => true;
    public int Capacity { get; private set; }
    public List<Card> Cards { get; private set; }
    public int ActiveCardIndex { get; set; }

    public Slot(IStorage.StorageNames name, IStorage.AvailableCardsTypes cardsType, int capacity)
    {
        StorageName = name;
        CardsTypes = cardsType;
        Capacity = capacity;
        Cards = new(Capacity);
        ActiveCardIndex = 0;
    }

    public Card AddCard(Card card)
    {
        if (Cards.Count < Capacity)
        {
            Cards.Add(card);
            return null;
        }
        var releasedCard = Cards[ActiveCardIndex];
        Cards[ActiveCardIndex] = card;
        return releasedCard;
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

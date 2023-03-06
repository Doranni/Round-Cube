using System;
using System.Collections.Generic;

public class Reward : IStorage
{
    public IStorage.StorageNames StorageName { get; private set; }
    public Card.CardsType CardTypes { get; private set; }
    public bool AffectsStats => false;
    public List<Card> Cards { get; private set; }

    public event Action CardsChanged;

    public Reward()
    {
        StorageName = IStorage.StorageNames.Reward;
        CardTypes = Card.CardsType.Weapon | Card.CardsType.Armor | Card.CardsType.Shield | Card.CardsType.Magic
            | Card.CardsType.Potion | Card.CardsType.Artifact;
        Cards = new();
    }

    public (bool success, Card releasedCard) AddCard(Card card, bool compareCardTypesFlags = false)
    {
        Cards.Add(card);
        CardsChanged?.Invoke();
        return (true, null);
    }

    public bool RemoveCard(Card card, bool forceRemove = false)
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

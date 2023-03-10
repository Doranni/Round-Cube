using System;
using System.Collections.Generic;

public class Inventory : IStorage
{
    private Character character;
    public IStorage.StorageNames StorageName { get; private set; }
    public Card.CardsType CardTypes { get; private set; }
    public List<Card> Cards { get; private set; }

    public event Action CardsChanged;

    public Inventory(Character character)
    {
        this.character = character;
        StorageName = IStorage.StorageNames.Inventory;
        CardTypes = Card.CardsType.Weapon | Card.CardsType.Armor | Card.CardsType.Shield | Card.CardsType.Magic 
            | Card.CardsType.Potion | Card.CardsType.Artifact;
        Cards = new();
    }

    public (bool, Card) AddCard(Card card)
    {
        Cards.Add(card);
        card.SetCardOwner(character, StorageName);
        CardsChanged?.Invoke();
        return (true, null);
    }

    public bool RemoveCard(Card card, bool forceRemove = false)
    {
        var cardToRemove = Cards.Find(x => x.InstanceId == card.InstanceId); ;
        if (cardToRemove == null)
        {
            return false;
        }
        Cards.Remove(cardToRemove);
        CardsChanged?.Invoke();
        return true;
    }
}

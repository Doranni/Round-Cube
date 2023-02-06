using System;
using System.Collections.Generic;
using UnityEngine;

public class Card 
{
    [Flags]
    public enum CardsType
    {
        Weapon = 1,
        Armor = 2,
        Shield = 4,
        Other = 8
    }

    public int Id { get; private set; }
    public CardsType CardType { get; private set; }
    public string CardName { get; private set; }
    public string Description { get; private set; }
    public List<StatBonus> StatBonuses { get; private set; }
    public int Duration { get; private set; }
    public Texture Image { get; private set; }

    public Card(CardData data)
    {
        Id = data.id;
        CardType = data.cardType;
        CardName = data.cardName;
        Description = data.description;
        StatBonuses = data.statBonuses;
        Duration = data.duration;
        Image = data.image;
    }
}

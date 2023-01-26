using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "Cards/AllCards")]
public class CardsDatabaseData : ScriptableObject
{
    public List<CardData> cards;
}

[Serializable]
public class CardData
{
    public Card.CardsType cardType;
    public string cardName;
    public string description;
    public List<StatBonus> statBonuses;
    public int duration;
    public Texture image;
}


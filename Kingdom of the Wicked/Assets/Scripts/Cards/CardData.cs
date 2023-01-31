using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "ScriptableObjects/Card")]
public class CardData : ScriptableObject
{
    public int id;
    public Card.CardsType cardType;
    public string cardName;
    public string description;
    public List<StatBonus> statBonuses;
    public int duration;
    public Texture image;
}


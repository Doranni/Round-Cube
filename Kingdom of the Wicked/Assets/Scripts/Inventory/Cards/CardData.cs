using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "card", menuName = "Cards/Card")]
public class CardData : ScriptableObject
{
    public string cardName;
    public string description;
    public List<StatBonus> StatBonuses;
    public int duration;
    public Texture image;
}

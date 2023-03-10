using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardSO : ScriptableObject
{
    [SerializeField] protected int id;
    [SerializeField] protected string cardName;
    [SerializeField] protected string description;
    [SerializeField] protected Card.CardEffectType effectType;
    [SerializeField] protected Texture image;

    public int Id => id;
    public string CardName => cardName;
    public string Description => description;
    public Card.CardEffectType EffectType => effectType;
    public Texture Image => image;
}


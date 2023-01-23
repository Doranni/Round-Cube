using System;
using System.Collections.Generic;
using UnityEngine;

public class Stat 
{
    public StatType Type { get; private set; }
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int BaseValue { get; private set; }
    public int TotalValue { get; private set; }
    public List<StatEffect> Effects { get; private set; }

    public Stat(StatType type, string name, string description, int baseValue)
    {
        Type = type;
        Name = name;
        Description = description;
        BaseValue = baseValue;
        TotalValue = BaseValue;
        Effects = new();
    }

    public void AddEffect(StatEffect effect)
    {
        Effects.Add(effect);
        TotalValue += effect.Value;
    }

    public void RemoveEffect(StatEffect effect)
    {
        Effects.Remove(effect);
        TotalValue -= effect.Value;
    }
}

public enum StatType
{
    health,
    armor,
    damage
}

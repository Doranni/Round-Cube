using System;
using System.Collections.Generic;
using UnityEngine;

public class Stat 
{
    public string StatName { get; private set; }
    public string Description { get; private set; }
    public int BaseValue { get; private set; }
    public int TotalValue { get; private set; }
    public Dictionary<int, StatBonus> Effects { get; private set; }

    public Stat(string statName, string description, int baseValue)
    {
        StatName = statName;
        Description = description;
        BaseValue = baseValue;
        TotalValue = BaseValue;
        Effects = new();
    }

    public void AddBonus(int id, StatBonus effect)
    {
        Effects.Add(id, effect);
        TotalValue += effect.Value;
    }

    public void RemoveBonus(int id)
    {
        TotalValue -= Effects[id].Value;
        Effects.Remove(id);
    }
}

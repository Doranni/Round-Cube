using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatBonus
{
    public string StatName { get; private set; }
    public int Value { get; private set; }

    public StatBonus(string statName, int value)
    {
        StatName = statName;
        Value = value;
    }
}

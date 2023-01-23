using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatEffect
{
    public StatType Type { get; private set; }
    public int Value { get; private set; }

    public StatEffect(StatType type, int value)
    {
        Type = type;
        Value = value;
    }
}

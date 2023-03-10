using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatBonus
{
    [SerializeField] private Stat.StatId statTypeId;
    [SerializeField] private int value;
    private int bonusId;

    public Stat.StatId StatTypeId => statTypeId;
    public int Value => value;  
    public int BonusId => bonusId;

    public void SetId(int id)
    {
        bonusId = id;
    }
}

[Serializable]
public class Effect
{
    public enum EffectTypes
    {
        Damage,
        Heal,
        FireDamage
    }

    public enum ValueTypes
    {
        value,
        percentage
    }

    [SerializeField] private EffectTypes effectType;
    [SerializeField] private string name;
    [SerializeField] private string description;
    [SerializeField] private int duration;
    [SerializeField] private ValueTypes valueType;
    [SerializeField] private float value;
    [SerializeField] [Range(0, 1)] private float chance;

    public int Id { get; private set; }
    public EffectTypes EffectType => effectType;
    public string Name => name;
    public string Description => description;
    public int Duration => duration;
    public ValueTypes ValueType => valueType;
    public float Value => value;
    public float Chance => chance;

    public Effect(EffectTypes effectType, string name, string description, int duration, ValueTypes valueType,
        float value, float chance)
    {
        this.effectType = effectType;
        this.name = name;
        this.description = description;
        this.duration = duration;
        this.valueType = valueType;
        this.value = value;
        this.chance = chance;
    }

    public void SetId()
    {
        if (Id == 0)
        {
            Id = GameManager.Instance.GetID();
        }
    }

    public void DecreaseDuration()
    {
        --duration;
    }

    public Effect Clone()
    {
        return new Effect(effectType, name, description, duration, valueType, value, chance);
    }
}

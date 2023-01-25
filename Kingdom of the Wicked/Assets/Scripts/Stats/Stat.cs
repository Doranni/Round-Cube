using System;
using System.Collections.Generic;
using UnityEngine;

public class Stat 
{
    public enum StatId
    {
        health,
        armor,
        damage
    }
    public StatId Id { get; private set; }
    public string StatName { get; private set; }
    public string Description { get; private set; }
    public int BaseValue { get; private set; }
    public int TotalValue { get; private set; }
    public Dictionary<int, StatBonus> Bonuses { get; private set; }

    public event Action<int> OnAddBonus, OnRemoveBonus;

    public Stat(string statName, string description, int baseValue)
    {
        StatName = statName;
        Description = description;
        BaseValue = baseValue;
        TotalValue = BaseValue;
        Bonuses = new();
    }

    public void AddBonus(StatBonus bonus)
    {
        int id = GameManager.Instance.GetID();
        Bonuses.Add(id, bonus);
        bonus.SetId(id);
        TotalValue += bonus.Value;
        OnAddBonus?.Invoke(bonus.Value);
    }

    public void RemoveBonus(StatBonus bonus)
    {
        if (Bonuses.ContainsKey(bonus.BonusId))
        {
            TotalValue -= bonus.Value;
            Bonuses.Remove(bonus.BonusId);
            OnRemoveBonus?.Invoke(bonus.Value);
        }
    }
}

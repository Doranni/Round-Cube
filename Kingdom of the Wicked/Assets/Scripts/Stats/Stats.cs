using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats
{
    public Dictionary<Stat.StatId, Stat> StatsValues { get; private set; }

    public Stats(StatsValuesSO statsValues)
    {
        StatsValues = new(GameDatabase.Instance.StatsDescription.Count)
        {
            { Stat.StatId.health, new(statsValues.baseHealthValue) },
            { Stat.StatId.armor, new(statsValues.armorValue) },
            { Stat.StatId.damage, new(statsValues.damageValue) }
        };
    }

    public void AddBonus(StatBonus bonus)
    {
        if (StatsValues.ContainsKey(bonus.StatTypeId))
        {
            StatsValues[bonus.StatTypeId].AddBonus(bonus);
        }
    }

    public void RemoveBonus(StatBonus bonus)
    {
        if (StatsValues.ContainsKey(bonus.StatTypeId))
        {
            StatsValues[bonus.StatTypeId].RemoveBonus(bonus);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private StatData healthStat, armorStat, damageStat;

    public Dictionary<StatType, Stat> Stats { get; private set; }

    private void Awake()
    {
        Stats = new();
        Stats.Add(StatType.health, new Stat(StatType.health, healthStat.statName, healthStat.description, 
            healthStat.baseValue));
        Stats.Add(StatType.armor, new Stat(StatType.armor, armorStat.statName, armorStat.description,
            armorStat.baseValue));
        Stats.Add(StatType.damage, new Stat(StatType.damage, damageStat.statName, damageStat.description,
            damageStat.baseValue));
    }
}

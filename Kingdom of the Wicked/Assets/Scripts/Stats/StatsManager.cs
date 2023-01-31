using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private StatValueData[] stats;

    public Dictionary<Stat.StatId, Stat> Stats { get; private set; }

    private void Awake()
    {
        Stats = new();
        for (int i = 0; i < stats.Length; i++)
        {
            Stats.Add(stats[i].stat.Id, new Stat(stats[i].stat.statName, stats[i].stat.description, 
                stats[i].baseValue));
        }
    }

    public void AddBonus(StatBonus bonus)
    {
        if (Stats.ContainsKey(bonus.StatTypeId))
        {
            Stats[bonus.StatTypeId].AddBonus(bonus);
        }
    }

    public void RemoveBonus(StatBonus bonus)
    {
        if (Stats.ContainsKey(bonus.StatTypeId))
        {
            Stats[bonus.StatTypeId].RemoveBonus(bonus);
        }
    }
}

[System.Serializable]
public class StatValueData
{
    public StatData stat;
    public int baseValue;
}

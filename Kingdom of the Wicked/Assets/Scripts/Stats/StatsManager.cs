using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private StatValueData[] stats;

    public Dictionary<string, Stat> Stats { get; private set; }

    private void Awake()
    {
        Stats = new();
        for (int i = 0; i < stats.Length; i++)
        {
            Stats.Add(stats[i].stat.statName, new Stat(stats[i].stat.statName, stats[i].stat.description, 
                stats[i].baseValue));
        }
    }

    public void AddBonus(int id, StatBonus bonus)
    {
        if (Stats.ContainsKey(bonus.StatName))
        {
            Stats[bonus.StatName].AddBonus(id, bonus);
        }
    }

    public void RemoveBonus(int id, StatBonus bonus)
    {
        if (Stats.ContainsKey(bonus.StatName))
        {
            Stats[bonus.StatName].RemoveBonus(id);
        }
    }
}

[System.Serializable]
public class StatValueData
{
    public StatData stat;
    public int baseValue;
}

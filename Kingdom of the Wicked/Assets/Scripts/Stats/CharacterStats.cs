using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] private StatsValuesData chStatsValues;

    public Health ChHealth { get; private set; }
    public Stats ChStats { get; private set; }
    
    private void Awake()
    {
        ChStats = new Stats(chStatsValues);
        ChHealth = new Health(ChStats.StatsValues[Stat.StatId.health].BaseValue);
    }

    private void Start()
    {
        ChStats.StatsValues[Stat.StatId.health].OnAddBonus += x => ChHealth.AddHealthBonus(x);
        ChStats.StatsValues[Stat.StatId.health].OnRemoveBonus += x => ChHealth.AddHealthBonus(-x);
    }
}

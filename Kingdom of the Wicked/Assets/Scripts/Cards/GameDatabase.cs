using System.Collections.Generic;
using UnityEngine;

public class GameDatabase : Singleton<GameDatabase>
{
    [SerializeField] StatsDescriptionData statsDescriptionData;

    public Dictionary<int, CardData> Cards { get; private set; }
    public Dictionary<Stat.StatId, (string name, string description)> StatsDescription { get; private set; }

    public override void Awake()
    {
        base.Awake();
        CardData[] data = Resources.LoadAll<CardData>("Cards");
        Cards = new(data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            Cards.Add(data[i].id, data[i]);
        }

        StatsDescription = new();
        StatsDescription.Add(Stat.StatId.health, (statsDescriptionData.healthStatName, 
            statsDescriptionData.healthStatDescription));
        StatsDescription.Add(Stat.StatId.armor, (statsDescriptionData.armorStatName,
            statsDescriptionData.armorStatDescription));
        StatsDescription.Add(Stat.StatId.damage, (statsDescriptionData.damageStatName,
            statsDescriptionData.damageStatDescription));
    }

    public Card GetCard(int id)
    {
        if (Cards.ContainsKey(id))
        {
            return new Card(Cards[id]);
        }
        return null;
    }
}

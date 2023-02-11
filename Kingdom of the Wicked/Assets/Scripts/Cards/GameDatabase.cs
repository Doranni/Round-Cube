using System.Collections.Generic;
using UnityEngine;

public class GameDatabase : Singleton<GameDatabase>
{
    [SerializeField] StatsDescriptionSO statsDescriptionData;

    public Dictionary<int, CardSO> Cards { get; private set; }
    public Dictionary<Stat.StatId, (string name, string description)> StatsDescription { get; private set; }

    public override void Awake()
    {
        base.Awake();
        CardSO[] data = Resources.LoadAll<CardSO>("Cards");
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

    public Card GetCard(int nameId)
    {
        if (Cards.ContainsKey(nameId))
        {
            if (Cards[nameId] is WeaponCardSO)
            {
                return new WeaponCard((WeaponCardSO)Cards[nameId]);
            }
            if (Cards[nameId] is ArmorCardSO)
            {
                return new ArmorCard((ArmorCardSO)Cards[nameId]);
            }
            if (Cards[nameId] is ShieldCardSO)
            {
                return new ShieldCard((ShieldCardSO)Cards[nameId]);
            }
            if (Cards[nameId] is MagicCardSO)
            {
                return new MagicCard((MagicCardSO)Cards[nameId]);
            }
            if (Cards[nameId] is PotionCardSO)
            {
                return new PotionCard((PotionCardSO)Cards[nameId]);
            }
            if (Cards[nameId] is ArtifactCardSO)
            {
                return new ArtifactCard((ArtifactCardSO)Cards[nameId]);

            }
        }
        return null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    private readonly Character character;
    public Health ChHealth { get; private set; }
    public Dictionary<Stat.StatId, Stat> ChStats { get; private set; }
    public List<Effect> Effects { get; private set; }

    public CharacterStats(Character character, CharacterSO characterSO)
    {
        this.character = character;
        ChStats = new(GameDatabase.Instance.StatsDescription.Count)
        {
            { Stat.StatId.health, new(characterSO.baseHealthValue) },
            { Stat.StatId.armor, new(characterSO.armorValue) },
            { Stat.StatId.damage, new(characterSO.damageValue) }
        };
        ChHealth = new Health(character, ChStats[Stat.StatId.health].BaseValue);
        Effects = new();

        ChStats[Stat.StatId.health].BonusAdded += x => ChHealth.AddHealthBonus(x);
        ChStats[Stat.StatId.health].BonusRemoved += x => ChHealth.AddHealthBonus(-x);
    }

    public void AddEffect(Effect effect)
    {
        if (effect.Duration <= 0)
        {
            return;
        }
        var targetEffect = effect.Clone();
        targetEffect.SetId();
        Effects.Add(targetEffect);
    }

    public void ExecuteEffects()
    {
        for (int i = 0; i < Effects.Count; i++)
        {
            var chance = Random.value;
            if (chance <= Effects[i].Chance)
            {
                float effectValue = 0;
                if (Effects[i].ValueType == Effect.ValueTypes.percentage)
                {
                    effectValue = ChHealth.CurrentHealth * Effects[i].Value;
                }
                else
                {
                    effectValue = Effects[i].Value;
                }
                switch (Effects[i].EffectType)
                {
                    case Effect.EffectTypes.damage:
                    case Effect.EffectTypes.fireDamage:
                        {
                            ChHealth.ChangeHealth(-effectValue);
                            break;
                        }
                    case Effect.EffectTypes.heal:
                        {
                            ChHealth.ChangeHealth(effectValue);
                            break;
                        }
                }
                Effects[i].DecreaseDuration();
                if (Effects[i].Duration <= 0)
                {
                    Effects.RemoveAt(i);
                    --i;
                }
            }
        }
    }

    public void AddBonus(StatBonus bonus)
    {
        if (ChStats.ContainsKey(bonus.StatTypeId))
        {
            ChStats[bonus.StatTypeId].AddBonus(bonus);
        }
    }

    public void RemoveBonus(StatBonus bonus)
    {
        if (ChStats.ContainsKey(bonus.StatTypeId))
        {
            ChStats[bonus.StatTypeId].RemoveBonus(bonus);
        }
    }
}

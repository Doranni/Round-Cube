using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private StatsValuesSO chStatsValues;

    public Health ChHealth { get; private set; }
    public Stats ChStats { get; private set; }
    public Dictionary<int, Effect> Effects { get; private set; }

    private void Awake()
    {
        ChStats = new Stats(chStatsValues);
        ChHealth = new Health(ChStats.StatsValues[Stat.StatId.health].BaseValue);
        Effects = new();
    }

    private void Start()
    {
        ChStats.StatsValues[Stat.StatId.health].BonusAdded += x => ChHealth.AddHealthBonus(x);
        ChStats.StatsValues[Stat.StatId.health].BonusRemoved += x => ChHealth.AddHealthBonus(-x);
    }

    public void AddEffect(Effect effect)
    {
        if (effect.Duration <= 0)
        {
            return;
        }
        Debug.Log($"Added effect {effect.Name} to {name}.");
        var targetEffect = effect.Clone();
        targetEffect.SetId();
        Effects.Add(targetEffect.Id, targetEffect);
    }

    private void ExecuteEffects()
    {
        foreach (Effect effect in Effects.Values)
        {
            var chance = Random.value;
            if (chance <= effect.Chance)
            {
                float effectValue = 0;
                if (effect.ValueType == Effect.ValueTypes.percentage)
                {
                    effectValue = ChHealth.CurrentHealth * effect.Value;
                }
                else
                {
                    effectValue = effect.Value;
                }
                switch (effect.EffectType)
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
                effect.DecreaseDuration();
                if (effect.Duration <= 0)
                {
                    Effects.Remove(effect.Id);
                }
            }
        }
    }

    private void UseCard(IUsable card, Character target)
    {
        card.Use(target);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats
{
    private readonly Character character;
    public Health ChHealth { get; private set; }
    public Dictionary<Stat.StatId, Stat> ChStats { get; private set; }
    public List<Effect> Effects { get; private set; }
    public ArmorCard armorCard;
    public ShieldCard shieldCard;

    public CharacterStats(Character character, CharacterSO characterSO)
    {
        this.character = character;
        ChStats = new(GameDatabase.Instance.StatsDescription.Count)
        {
            { Stat.StatId.health, new(characterSO.BaseHealthValue) },
        };
        ChHealth = new Health(character, ChStats[Stat.StatId.health].BaseValue);
        Effects = new();

        ChStats[Stat.StatId.health].BonusAdded += x => ChHealth.AddHealthBonus(x);
        ChStats[Stat.StatId.health].BonusRemoved += x => ChHealth.AddHealthBonus(-x);
    }

    public void UseCard(ICardUsable card)
    {
        if (card is WeaponCard && shieldCard != null)
        {
            if (shieldCard.BlockChargesLeft > 0 && shieldCard.BlockChanse >= Random.value)
            {
                shieldCard.DecreaseCharges();
                return;
            }
        }
        foreach (Effect effect in card.Effects)
        {
            if (effect.Duration < 0)
            {
                return;
            }
            if (effect.Duration == 0)
            {
                ExecuteEffect(effect);
                return;
            }
            var targetEffect = effect.Clone();
            targetEffect.SetId();
            Effects.Add(targetEffect);
        }
        card.WasUsed();
    }

    public void SetArmor(ArmorCard card)
    {
        armorCard = card;
    }

    public void SetShield(ShieldCard card)
    {
        shieldCard = card;
    }

    public void ExecuteEffects()
    {
        for (int i = 0; i < Effects.Count; i++)
        {
            ExecuteEffect(Effects[i]);
            Effects[i].DecreaseDuration();
            if (Effects[i].Duration <= 0)
            {
                Effects.RemoveAt(i);
                --i;
            }
        }
    }

    private void ExecuteEffect(Effect effect)
    {
        if (effect.Chance == 1 || effect.Chance >= Random.value)
        {
            float effectValue;
            if (effect.ValueType == Effect.ValueTypes.percentage)
            {
                effectValue = ChHealth.MaxHealth * effect.Value;
            }
            else
            {
                effectValue = effect.Value;
            }
            switch (effect.EffectType)
            {
                case Effect.EffectTypes.Damage:
                case Effect.EffectTypes.FireDamage:
                    {
                        if (armorCard != null && armorCard.Protection > 0)
                        {
                            if (effectValue > armorCard.Protection)
                            {
                                effectValue -= armorCard.Protection;
                                armorCard.DecreaseProtection(armorCard.Protection);
                                ChHealth.ChangeHealth(-effectValue);
                            }
                            else
                            {
                                armorCard.DecreaseProtection((int)effectValue);
                            }
                            return;
                        }
                        ChHealth.ChangeHealth(-effectValue);
                        break;
                    }
                case Effect.EffectTypes.Heal:
                    {
                        ChHealth.ChangeHealth(effectValue);
                        break;
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

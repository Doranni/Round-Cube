using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card 
{
    [Flags]
    public enum CardsType
    {
        Weapon = 1,
        Armor = 2,
        Shield = 4,
        Magic = 8,
        Potion = 16,
        Artifact = 32
    }

    public int NameId { get; protected set; }
    public int InstanceId { get; protected set; }
    public CardsType CardType { get; protected set; }
    public string CardName { get; protected set; }
    public string Description { get; protected set; }  
    public int Duration { get; protected set; }
    public Texture Image { get; protected set; }

    public Card(CardSO cardSO)
    {
        NameId = cardSO.id;
        CardName = cardSO.cardName;
        Description = cardSO.description;
        Duration = cardSO.duration;
        Image = cardSO.image;
    }

    public void SetInstanceId()
    {
        if (InstanceId == 0)
        {
            InstanceId = GameManager.Instance.GetID();
        }
    }

    public static bool ComperaCardTypesFlags(CardsType flags1, CardsType flags2)
    {
        if ((flags1.HasFlag(CardsType.Weapon) && flags2.HasFlag(CardsType.Weapon))
            || (flags1.HasFlag(CardsType.Armor) && flags2.HasFlag(CardsType.Armor))
            || (flags1.HasFlag(CardsType.Shield) && flags2.HasFlag(CardsType.Shield))
            || (flags1.HasFlag(CardsType.Magic) && flags2.HasFlag(CardsType.Magic))
            || (flags1.HasFlag(CardsType.Potion) && flags2.HasFlag(CardsType.Potion))
            || (flags1.HasFlag(CardsType.Artifact) && flags2.HasFlag(CardsType.Artifact)))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

public interface IUsable
{
    public List<Effect> Effects { get; }
    public void Use(Character target)
    {
        foreach(Effect effect in Effects)
        {
            target.Stats.AddEffect(effect);
        }
    }
}

public interface IAddStatBonuses
{
    public List<StatBonus> StatBonuses { get; }
}

public class WeaponCard : Card, IUsable, IAddStatBonuses
{
    public List<Effect> Effects { get; private set; }
    public List<StatBonus> StatBonuses { get; private set; }

    public WeaponCard(WeaponCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Weapon;
        Effects = cardSO.effects;
        StatBonuses = cardSO.statBonuses;
    }
}

public class ArmorCard : Card, IAddStatBonuses
{
    public List<StatBonus> StatBonuses { get; private set; }

    public ArmorCard(ArmorCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Armor;
        StatBonuses = cardSO.statBonuses;
    }
}

public class ShieldCard : Card, IAddStatBonuses
{
    public List<StatBonus> StatBonuses { get; private set; }

    public ShieldCard(ShieldCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Shield;
        StatBonuses = cardSO.statBonuses;
    }
}

public class PotionCard : Card, IUsable
{
    public List<Effect> Effects { get; protected set; }

    public PotionCard(PotionCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Potion;
        Effects = cardSO.effects;
    }
}

public class MagicCard : Card, IUsable
{
    public List<Effect> Effects { get; protected set; }

    public MagicCard(MagicCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Magic;
        Effects = cardSO.effects;
    }
}

public class ArtifactCard : Card
{
    public ArtifactCard(ArtifactCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Artifact;
    }
}

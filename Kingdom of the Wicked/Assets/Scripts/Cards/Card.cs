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

    public enum CardEffectType
    {
        Harm,
        Benefit
    }

    public int NameId { get; protected set; }
    public int InstanceId { get; protected set; }
    public CardsType CardType { get; protected set; }
    public CardEffectType EffectType { get; protected set; }
    public string CardName { get; protected set; }
    public string Description { get; protected set; }  
    public Texture Image { get; protected set; }
    public Character Owner { get; protected set; }
    public IStorage.StorageNames Storage { get; protected set; }

    public Action<bool> WasSelected, WasHided;
    public Action WasChanged;

    public Card(CardSO cardSO)
    {
        NameId = cardSO.Id;
        CardName = cardSO.CardName;
        Description = cardSO.Description;
        EffectType = cardSO.EffectType;
        Image = cardSO.Image;
    }

    public void SetCardOwner(Character character, IStorage.StorageNames storage)
    {
        Owner = character;
        Storage = storage;
        if (InstanceId == 0)
        {
            InstanceId = GameManager.Instance.GetID();
        }
    }

    public void SelectCard(bool value)
    {
        WasSelected?.Invoke(value);
    }

    public void HideCard(bool value)
    {
        WasHided?.Invoke(value);
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

public interface ICardUsable
{
    public List<Effect> Effects { get; }
    public void WasUsed();
}

public interface ICardAddStatBonuses
{
    public List<StatBonus> StatBonuses { get; }
}

public interface ICardBreakable
{
    public int ChargesMax { get; }
    public int ChargesLeft { get; }
    public void SetChargesLeft(int value);
}

public class WeaponCard : Card, ICardUsable, ICardAddStatBonuses
{
    public List<Effect> Effects { get; private set; }
    public List<StatBonus> StatBonuses { get; private set; }

    public WeaponCard(WeaponCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Weapon;
        Effects = cardSO.Effects;
        StatBonuses = cardSO.StatBonuses;
    }

    public void WasUsed() { }
}

public class ArmorCard : Card, ICardAddStatBonuses
{
    public int Protection;
    public List<StatBonus> StatBonuses { get; private set; }

    public ArmorCard(ArmorCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Armor;
        Protection = cardSO.Protection;
        StatBonuses = cardSO.StatBonuses;
    }

    public void DecreaseProtection(int value)
    {
        Protection -= value;
        WasChanged?.Invoke();
    }

    public void SetProtection(int value)
    {
        Protection = value;
        WasChanged?.Invoke();
    }
}

public class ShieldCard : Card, ICardAddStatBonuses
{
    public int BlockChargesMax { get; private set; }
    public int BlockChargesLeft { get; private set; }
    public float BlockChanse { get; private set; }
    public List<StatBonus> StatBonuses { get; private set; }

    public ShieldCard(ShieldCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Shield;
        StatBonuses = cardSO.StatBonuses;
        BlockChargesMax = cardSO.BlockCharges;
        BlockChargesLeft = BlockChargesMax;
        BlockChanse = cardSO.BlockChanse;
    }

    public void DecreaseCharges()
    {
        --BlockChargesLeft;
        WasChanged?.Invoke();
    }

    public void SetChargesLeft(int value)
    {
        BlockChargesLeft = Mathf.Clamp(value, 0, BlockChargesMax);
        WasChanged?.Invoke();
    }
}

public class MagicCard : Card, ICardUsable, ICardBreakable
{
    public List<Effect> Effects { get; protected set; }
    public int ChargesMax { get; protected set; }
    public int ChargesLeft { get; protected set; }

    public MagicCard(MagicCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Magic;
        Effects = cardSO.Effects;
        ChargesMax = cardSO.Charges;
        ChargesLeft = ChargesMax;
    }

    public void WasUsed()
    {
        ChargesLeft--;
        if (ChargesLeft <= 0)
        {
            Owner.Equipment.RemoveCard(this, Storage);
        }
        WasChanged?.Invoke();
    }

    public void SetChargesLeft(int value)
    {
        ChargesLeft = Mathf.Clamp(value, 0, ChargesMax);
        WasChanged?.Invoke();
    }
}

public class PotionCard : Card, ICardUsable, ICardBreakable
{
    public List<Effect> Effects { get; protected set; }
    public int ChargesMax { get; protected set; }
    public int ChargesLeft { get; protected set; }

    public PotionCard(PotionCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Potion;
        Effects = cardSO.Effects;
        ChargesMax = cardSO.Charges;
        ChargesLeft = ChargesMax;
    }

    public void WasUsed()
    {
        ChargesLeft--;
        if (ChargesLeft <= 0)
        {
            Owner.Equipment.RemoveCard(this, Storage);
        }
        WasChanged?.Invoke();
    }

    public void SetChargesLeft(int value)
    {
        ChargesLeft = Mathf.Clamp(value, 0, ChargesMax);
        WasChanged?.Invoke();
    }
}

public class ArtifactCard : Card
{
    public ArtifactCard(ArtifactCardSO cardSO) : base(cardSO)
    {
        CardType = CardsType.Artifact;
    }
}

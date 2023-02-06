using System;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;

    public Dictionary<IStorage.StorageNames, IStorage> Storages { get; private set; }

    public event Action<IStorage.StorageNames> OnStorageChanged;

    private void Awake()
    {
        Storages = new();
        Storages.Add(IStorage.StorageNames.weaponSlot, new Slot(IStorage.StorageNames.weaponSlot,
            Card.CardsType.Weapon, 1));
        Storages.Add(IStorage.StorageNames.armorSlot, new Slot(IStorage.StorageNames.armorSlot,
            Card.CardsType.Armor, 1));
        Storages.Add(IStorage.StorageNames.shieldSlot, new Slot(IStorage.StorageNames.shieldSlot,
            Card.CardsType.Shield, 1));
        Storages.Add(IStorage.StorageNames.otherSlot, new Slot(IStorage.StorageNames.otherSlot,
            Card.CardsType.Other, GameManager.Instance.Equipment_OtherSlotCapacity));
        Storages.Add(IStorage.StorageNames.inventory, new Inventory());
    }

    public void AddCard(Card card, IStorage.StorageNames storageName)
    {
        var releasedCard = Storages[storageName].AddCard(card);
        if (releasedCard != null)
        {
            Storages[IStorage.StorageNames.inventory].AddCard(releasedCard);
        }
        if (Storages[storageName].AffectsStats)
        {
            foreach (StatBonus bonus in card.StatBonuses)
            {
                characterStats.ChStats.AddBonus(bonus);
            }
            if (releasedCard != null)
            {
                foreach (StatBonus bonus in releasedCard.StatBonuses)
                {
                    characterStats.ChStats.RemoveBonus(bonus);
                }
            }
        }
        OnStorageChanged?.Invoke(storageName);
        if (releasedCard != null)
        {
            OnStorageChanged?.Invoke(IStorage.StorageNames.inventory);
        }
    }

    public void RemoveCard(Card card, IStorage.StorageNames storageName)
    {
        var success = Storages[storageName].RemoveCard(card);
        if (success && Storages[storageName].AffectsStats)
        {
            foreach (StatBonus bonus in card.StatBonuses)
            {
                characterStats.ChStats.RemoveBonus(bonus);
            }
        }
        if (success)
        {
            OnStorageChanged?.Invoke(storageName);
        }
    }

    public void MoveCard(Card card, IStorage.StorageNames prevSlot, IStorage.StorageNames newSlot)
    {
        AddCard(card, newSlot);
        RemoveCard(card, prevSlot);
    }
}

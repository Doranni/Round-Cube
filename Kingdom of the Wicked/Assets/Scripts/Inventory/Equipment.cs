using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class Equipment : MonoBehaviour
{
    public Dictionary<IStorage.StorageNames, IStorage> Storages { get; private set; }

    private StatsManager statsManager;

    public event Action<IStorage.StorageNames> OnStorageChanged;

    private void Awake()
    {
        statsManager = GetComponent<StatsManager>();

        Storages = new();
        Storages.Add(IStorage.StorageNames.weaponSlot, new Slot(IStorage.StorageNames.weaponSlot, 
            IStorage.AvailableCardsTypes.weapon, 1));
        Storages.Add(IStorage.StorageNames.armorSlot, new Slot(IStorage.StorageNames.armorSlot,
            IStorage.AvailableCardsTypes.armor, 1));
        Storages.Add(IStorage.StorageNames.otherSlot, new Slot(IStorage.StorageNames.otherSlot,
            IStorage.AvailableCardsTypes.other, GameManager.Instance.Equipment_OtherSlotCapacity));
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
                statsManager.AddBonus(bonus);
            }
            if (releasedCard != null)
            {
                foreach (StatBonus bonus in releasedCard.StatBonuses)
                {
                    statsManager.RemoveBonus(bonus);
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
                statsManager.RemoveBonus(bonus);
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

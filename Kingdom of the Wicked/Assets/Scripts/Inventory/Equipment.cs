using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class Equipment : MonoBehaviour
{
    public Dictionary<Storage.StorageNames, Storage> Storages { get; private set; }

    private StatsManager statsManager;

    private void Awake()
    {
        statsManager = GetComponent<StatsManager>();

        Storages = new();
        Storages.Add(Storage.StorageNames.weaponSlot, new Storage(Storage.StorageNames.weaponSlot, 
            Storage.AvailableCardsTypes.weapon, true, 1));
        Storages.Add(Storage.StorageNames.armorSlot, new Storage(Storage.StorageNames.armorSlot,
            Storage.AvailableCardsTypes.armor, true, 1));
        Storages.Add(Storage.StorageNames.otherSlot, new Storage(Storage.StorageNames.otherSlot,
            Storage.AvailableCardsTypes.other, true, GameManager.Instance.Equipment_OtherSlotCapacity));
        Storages.Add(Storage.StorageNames.inventory, new Storage(Storage.StorageNames.inventory,
            Storage.AvailableCardsTypes.weapon | Storage.AvailableCardsTypes.armor | Storage.AvailableCardsTypes.other, 
            false, GameManager.Instance.Equipment_InventoryCapacity));
    }

    public void AddCard(Card card, Storage.StorageNames storageName)
    {
        // to check is slot is empty
        Storages[storageName].AddCard(card);
        if (Storages[storageName].AffectStats)
        {
            foreach (StatBonus bonus in card.StatBonuses)
            {
                statsManager.AddBonus(bonus);
            }
        }
    }

    public void RemoveCard(Card card, Storage.StorageNames storageName)
    {
        Storages[storageName].RemoveCard(card);
        //to think how to recalc stats
        if (Storages[storageName].AffectStats)
        {
            foreach (StatBonus bonus in card.StatBonuses)
            {
                statsManager.RemoveBonus(bonus);
            }
        }
    }

    public void MoveCard(Card card, Storage.StorageNames prevSlot, Storage.StorageNames newSlot)
    {
        AddCard(card, newSlot);
        RemoveCard(card, prevSlot);
        //to think how to recalc stats
    }
}

public class Storage{
    [Flags]
    public enum AvailableCardsTypes
    {
        weapon = 1,
        armor = 2,
        other = 4
    }
    public enum StorageNames
    {
        weaponSlot,
        armorSlot,
        otherSlot,
        inventory,
        storage
    }

    public StorageNames StorageName { get; private set; }
    public AvailableCardsTypes CardsTypes { get; private set; }
    public bool AffectStats { get; private set; }
    public int Capacity { get; private set; }
    public int ActiveSlot { get; private set; }
    public List<Card> Cards { get; private set; }

    public event Action<List<Card>> OnStorageChanged;

    public Storage(StorageNames name, AvailableCardsTypes type, bool affectStats, int capacity)
    {
        StorageName = name;
        CardsTypes = type;
        AffectStats = affectStats;
        Capacity = capacity;
        Cards = new(Capacity);
        ActiveSlot = -1;
    }

    public void AddCard(Card card)
    {
        Cards.Add(card);
        OnStorageChanged?.Invoke(Cards);
    }

    public bool RemoveCard(Card card)
    {
        var cardToRemove = Cards.Find(x => x.Id == card.Id);
        if (cardToRemove == null)
        {
            return false;
        }
        Cards.Remove(cardToRemove);
        OnStorageChanged?.Invoke(Cards);
        return true;
    }
}

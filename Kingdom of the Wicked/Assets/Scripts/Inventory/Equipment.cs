using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class Equipment : MonoBehaviour
{
    public Dictionary<Storage.StorageNames, Storage> slots = new();

    public Card WeaponCard { get; private set; }
    public Card ArmorCard { get; private set; }
    public Dictionary<int, Card> OtherCards { get; private set; }

    private int activeOtherSlot = 0;
    public int ActiveOtherSlot => activeOtherSlot;

    private StatsManager statsManager;

    public event Action OnEquippedWeaponCardChanged, OnEquippedArmorCardChanged,
        OnEquippedOtherCardChanged;

    private void Awake()
    {
        statsManager = GetComponent<StatsManager>();

        slots.Add(Storage.StorageNames.weaponSlot, new Storage(Storage.StorageNames.weaponSlot, 
            Storage.AvailableCardsTypes.weapon, true, 1));
        slots.Add(Storage.StorageNames.armorSlot, new Storage(Storage.StorageNames.armorSlot,
            Storage.AvailableCardsTypes.armor, true, 1));
        slots.Add(Storage.StorageNames.otherSlot, new Storage(Storage.StorageNames.otherSlot,
            Storage.AvailableCardsTypes.other, true, GameManager.Instance.Equipment_OtherSlotCapacity));
        slots.Add(Storage.StorageNames.inventory, new Storage(Storage.StorageNames.inventory,
            Storage.AvailableCardsTypes.weapon | Storage.AvailableCardsTypes.armor | Storage.AvailableCardsTypes.other, 
            true, GameManager.Instance.Equipment_InventoryCapacity));

        OtherCards = new Dictionary<int, Card>(GameManager.Instance.Equipment_OtherSlotCapacity);
        for (int i = 0; i < GameManager.Instance.Equipment_OtherSlotCapacity; i++)
        {
            OtherCards.Add(i, null);
        }
    }

    public void EquipWeaponCard(Card card)
    {
        if (WeaponCard != null)
        {
            foreach (StatBonus bonus in WeaponCard.StatBonuses)
            {
                statsManager.RemoveBonus(bonus);
            }
        }
        WeaponCard = card;
        foreach(StatBonus bonus in card.StatBonuses)
        {
            statsManager.AddBonus(bonus);
        }
        OnEquippedWeaponCardChanged?.Invoke();
    }

    public void EquipArmorCard(Card card)
    {
        if (ArmorCard != null)
        {
            foreach (StatBonus bonus in ArmorCard.StatBonuses)
            {
                statsManager.RemoveBonus(bonus);
            }
        }
        ArmorCard = card;
        foreach (StatBonus bonus in card.StatBonuses)
        {
            statsManager.AddBonus(bonus);
        }
        OnEquippedArmorCardChanged?.Invoke();
    }

    public void EquipOtherCard(Card card)
    {
        if (OtherCards[activeOtherSlot] != null)
        {
            foreach (StatBonus bonus in OtherCards[activeOtherSlot].StatBonuses)
            {
                statsManager.RemoveBonus(bonus);
            }
        }
        OtherCards[activeOtherSlot] = card;
        foreach (StatBonus bonus in card.StatBonuses)
        {
            statsManager.AddBonus(bonus);
        }
        OnEquippedOtherCardChanged?.Invoke();
    }

    public void MoveCard(Storage.StorageNames prevSlot, Storage.StorageNames newSlot)
    {

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
    public bool IsActive { get; private set; }
    public int Capacity { get; private set; }
    public int ActiveSlot { get; private set; }
    public List<Card> Cards { get; private set; }

    public Storage(StorageNames name, AvailableCardsTypes type, bool isActive, int capacity)
    {
        StorageName = name;
        CardsTypes = type;
        IsActive = isActive;
        Capacity = capacity;
    }
}

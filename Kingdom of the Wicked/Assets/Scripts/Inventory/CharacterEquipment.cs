using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment : MonoBehaviour
{
    [SerializeField] private CharacterStats characterStats;

    public Dictionary<IStorage.StorageNames, IStorage> Storages { get; private set; }
    public Dictionary<SlotsHolder.SlotsHolderNames, SlotsHolder> SlotsHolders { get; private set; }

    private void Awake()
    {
        Storages = new();
        SlotsHolders = new();
        Storages.Add(IStorage.StorageNames.WeaponSlot, 
            new Slot(IStorage.StorageNames.WeaponSlot, Card.CardsType.Weapon));

        Storages.Add(IStorage.StorageNames.ArmorSlot,
            new Slot(IStorage.StorageNames.ArmorSlot, Card.CardsType.Armor));
        Storages.Add(IStorage.StorageNames.ShieldSlot,
            new Slot(IStorage.StorageNames.ShieldSlot, Card.CardsType.Shield));
        SlotsHolders.Add(SlotsHolder.SlotsHolderNames.Armor, new(SlotsHolder.SlotsHolderNames.Armor, new()
        {
            (Slot)Storages[IStorage.StorageNames.ArmorSlot],
            (Slot)Storages[IStorage.StorageNames.ShieldSlot]
        }));

        Storages.Add(IStorage.StorageNames.OtherSlot1,
            new Slot(IStorage.StorageNames.OtherSlot1, Card.CardsType.Other));
        Storages.Add(IStorage.StorageNames.OtherSlot2,
            new Slot(IStorage.StorageNames.OtherSlot2, Card.CardsType.Other));
        Storages.Add(IStorage.StorageNames.OtherSlot3,
            new Slot(IStorage.StorageNames.OtherSlot3, Card.CardsType.Other));
        SlotsHolders.Add(SlotsHolder.SlotsHolderNames.Other, new(SlotsHolder.SlotsHolderNames.Other, new()
        {
            (Slot)Storages[IStorage.StorageNames.OtherSlot1],
            (Slot)Storages[IStorage.StorageNames.OtherSlot2],
            (Slot)Storages[IStorage.StorageNames.OtherSlot3]
        }));

        Storages.Add(IStorage.StorageNames.Inventory, new Inventory());
    }

    public void AddCard(Card card, IStorage.StorageNames storageName)
    {
        // TODO: to fix it
        var releasedCard = Storages[storageName].AddCard(card);
        if (releasedCard != null)
        {
            Storages[IStorage.StorageNames.Inventory].AddCard(releasedCard);
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
        card.SetInstanceId(GameManager.Instance.GetID());
    }

    public void RemoveCard(Card card, IStorage.StorageNames storageName)
    {
        // TODO: to fix it
        var success = Storages[storageName].RemoveCard(card);
        if (success && Storages[storageName].AffectsStats)
        {
            foreach (StatBonus bonus in card.StatBonuses)
            {
                characterStats.ChStats.RemoveBonus(bonus);
            }
        }
    }

    public void MoveCard(Card card, IStorage.StorageNames prevSlot, IStorage.StorageNames newSlot)
    {
        if (Storages[prevSlot].RemoveCard(card))
        {
            var releasedCard = Storages[newSlot].AddCard(card);
            if (releasedCard != null)
            {
                Storages[IStorage.StorageNames.Inventory].AddCard(releasedCard);
            }
        }
        // TODO: to fix it
        
    }
}

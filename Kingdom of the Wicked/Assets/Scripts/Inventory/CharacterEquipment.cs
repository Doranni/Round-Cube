using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment
{
    private CharacterStats characterStats;
    public Dictionary<IStorage.StorageNames, IStorage> Storages { get; private set; }
    public Dictionary<SlotsHolder.SlotsHolderNames, SlotsHolder> SlotsHolders { get; private set; }

    public CharacterEquipment(CharacterStats characterStats)
    {
        this.characterStats = characterStats;
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

        Storages.Add(IStorage.StorageNames.OtherSlot1, new Slot(IStorage.StorageNames.OtherSlot1, 
            Card.CardsType.Magic | Card.CardsType.Potion | Card.CardsType.Artifact));
        Storages.Add(IStorage.StorageNames.OtherSlot2, new Slot(IStorage.StorageNames.OtherSlot2, 
            Card.CardsType.Magic | Card.CardsType.Potion | Card.CardsType.Artifact));
        Storages.Add(IStorage.StorageNames.OtherSlot3, new Slot(IStorage.StorageNames.OtherSlot3, 
            Card.CardsType.Magic | Card.CardsType.Potion | Card.CardsType.Artifact));
        SlotsHolders.Add(SlotsHolder.SlotsHolderNames.Other, new(SlotsHolder.SlotsHolderNames.Other, new()
        {
            (Slot)Storages[IStorage.StorageNames.OtherSlot1],
            (Slot)Storages[IStorage.StorageNames.OtherSlot2],
            (Slot)Storages[IStorage.StorageNames.OtherSlot3]
        }));

        Storages.Add(IStorage.StorageNames.Inventory, new Inventory());
    }

    public bool AddCard(Card card, IStorage.StorageNames storageName, bool compareCardTypesFlags = true)
    {
        var addRes = Storages[storageName].AddCard(card, compareCardTypesFlags);
        if (!addRes.success)
        {
            return false;
        }
        if (addRes.releasedCard != null)
        {
            Storages[IStorage.StorageNames.Inventory].AddCard(addRes.releasedCard);
        }
        if (Storages[storageName].AffectsStats && card is IAddStatBonuses)
        {
            foreach (StatBonus bonus in ((IAddStatBonuses)card).StatBonuses)
            {
                characterStats.AddBonus(bonus);
            }
            if (addRes.releasedCard != null && addRes.releasedCard is IAddStatBonuses)
            {
                foreach (StatBonus bonus in ((IAddStatBonuses)addRes.releasedCard).StatBonuses)
                {
                    characterStats.RemoveBonus(bonus);
                }
            }
        }
        card.SetInstanceId();
        return true;
    }

    public void RemoveCard(Card card, IStorage.StorageNames storageName)
    {
        var success = Storages[storageName].RemoveCard(card);
        if (success && Storages[storageName].AffectsStats && card is IAddStatBonuses)
        {
            foreach (StatBonus bonus in ((IAddStatBonuses)card).StatBonuses)
            {
                characterStats.RemoveBonus(bonus);
            }
        }
    }

    public bool MoveCard(Card card, IStorage.StorageNames prevSlot, IStorage.StorageNames newSlot, 
        bool compareCardTypesFlags = true)
    {
        if (Storages[prevSlot].RemoveCard(card))
        {
            var addRes = Storages[newSlot].AddCard(card, compareCardTypesFlags);
            if (!addRes.success)
            {
                var returnRes = Storages[prevSlot].AddCard(card, compareCardTypesFlags);
                if (!returnRes.success)
                {
                    Storages[IStorage.StorageNames.Inventory].AddCard(card);
                }
                return false;
            }
            if (addRes.releasedCard != null)
            {
                Storages[prevSlot].AddCard(addRes.releasedCard);
            }
            if (Storages[prevSlot].AffectsStats && !Storages[newSlot].AffectsStats)
            {
                if (addRes.releasedCard != null && addRes.releasedCard is IAddStatBonuses)
                {
                    foreach (StatBonus bonus in ((IAddStatBonuses)addRes.releasedCard).StatBonuses)
                    {
                        characterStats.AddBonus(bonus);
                    }
                }
                if (card is IAddStatBonuses)
                {
                    foreach (StatBonus bonus in ((IAddStatBonuses)card).StatBonuses)
                    {
                        characterStats.RemoveBonus(bonus);
                    }
                }
            }
            if (!Storages[prevSlot].AffectsStats && Storages[newSlot].AffectsStats)
            {
                if (card is IAddStatBonuses)
                {
                    foreach (StatBonus bonus in ((IAddStatBonuses)card).StatBonuses)
                    {
                        characterStats.AddBonus(bonus);
                    }
                }
                if (addRes.releasedCard != null && addRes.releasedCard is IAddStatBonuses)
                {
                    foreach (StatBonus bonus in ((IAddStatBonuses)addRes.releasedCard).StatBonuses)
                    {
                        characterStats.RemoveBonus(bonus);
                    }
                }
            }
            Debug.Log($"Card {card.CardName} was moved from {prevSlot} to {newSlot}");
            return true;
        }
        if (Storages[prevSlot] is Slot && Storages[newSlot] is Slot && Storages[newSlot].Cards.Count == 1)
        {
            var addRes = Storages[newSlot].AddCard(card, compareCardTypesFlags);
            if (!addRes.success)
            {
                return false;
            }
            if (!Storages[prevSlot].AddCard(addRes.releasedCard, compareCardTypesFlags).success)
            {
                if (!Storages[newSlot].AddCard(addRes.releasedCard, compareCardTypesFlags).success)
                {
                    Storages[IStorage.StorageNames.Inventory].AddCard(addRes.releasedCard);
                }
                return false;
            }
            Debug.Log($"Card {card.CardName} was moved from {prevSlot} to {newSlot}");
            return true;
        }
        return false;
    }
}

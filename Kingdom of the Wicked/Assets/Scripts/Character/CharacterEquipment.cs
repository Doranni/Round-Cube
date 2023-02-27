using System.Collections.Generic;
using UnityEngine;

public class CharacterEquipment
{
    private readonly Character character;
    public Dictionary<IStorage.StorageNames, IStorage> Storages { get; private set; }
    public Dictionary<SlotsHolder.SlotsHolderNames, SlotsHolder> SlotsHolders { get; private set; }

    public CharacterEquipment(Character character)
    {
        this.character = character;
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

    public bool AddCard(Card card, IStorage.StorageNames storageName, bool compareCardTypesFlags = true, 
        bool needToSave = true)
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
                character.Stats.AddBonus(bonus);
            }
            if (addRes.releasedCard != null && addRes.releasedCard is IAddStatBonuses)
            {
                foreach (StatBonus bonus in ((IAddStatBonuses)addRes.releasedCard).StatBonuses)
                {
                    character.Stats.RemoveBonus(bonus);
                }
            }
        }
        card.SetInstanceId();
        if (needToSave)
        {
            Save();
        }
        return true;
    }

    public void RemoveCard(Card card, IStorage.StorageNames storageName)
    {
        var success = Storages[storageName].RemoveCard(card);
        if (success)
        {
            Save();
            if (Storages[storageName].AffectsStats && card is IAddStatBonuses)
            {
                foreach (StatBonus bonus in ((IAddStatBonuses)card).StatBonuses)
                {
                    character.Stats.RemoveBonus(bonus);
                }
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
                    Save();
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
                        character.Stats.AddBonus(bonus);
                    }
                }
                if (card is IAddStatBonuses)
                {
                    foreach (StatBonus bonus in ((IAddStatBonuses)card).StatBonuses)
                    {
                        character.Stats.RemoveBonus(bonus);
                    }
                }
            }
            if (!Storages[prevSlot].AffectsStats && Storages[newSlot].AffectsStats)
            {
                if (card is IAddStatBonuses)
                {
                    foreach (StatBonus bonus in ((IAddStatBonuses)card).StatBonuses)
                    {
                        character.Stats.AddBonus(bonus);
                    }
                }
                if (addRes.releasedCard != null && addRes.releasedCard is IAddStatBonuses)
                {
                    foreach (StatBonus bonus in ((IAddStatBonuses)addRes.releasedCard).StatBonuses)
                    {
                        character.Stats.RemoveBonus(bonus);
                    }
                }
            }
            Debug.Log($"Card {card.CardName} was moved from {prevSlot} to {newSlot}");
            Save();
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
                    Save();
                    Storages[IStorage.StorageNames.Inventory].AddCard(addRes.releasedCard);
                }
                return false;
            }
            Debug.Log($"Card {card.CardName} was moved from {prevSlot} to {newSlot}");
            Save();
            return true;
        }
        return false;
    }

    public void EquipCard(Card card)
    {
        switch (card.CardType)
        {
            case Card.CardsType.Weapon:
                {
                    AddCard(card, IStorage.StorageNames.WeaponSlot, false, true);
                    break;
                }
            case Card.CardsType.Armor:
                {
                    AddCard(card, IStorage.StorageNames.ArmorSlot, false, true);
                    break;
                }
            case Card.CardsType.Shield:
                {
                    AddCard(card, IStorage.StorageNames.ShieldSlot, false, true);
                    break;
                }
            default:
                {
                    if (Storages[IStorage.StorageNames.OtherSlot1].Cards.Count == 0)
                    {
                        AddCard(card, IStorage.StorageNames.OtherSlot1, false, true);
                    }
                    else if (Storages[IStorage.StorageNames.OtherSlot2].Cards.Count == 0)
                    {
                        AddCard(card, IStorage.StorageNames.OtherSlot2, false, true);
                    }
                    else
                    {
                        AddCard(card, IStorage.StorageNames.OtherSlot3, false, true);
                    }
                    break;
                }
        }
    }

    private void Save()
    {
        SavesManager.Instance.UpdateCharacter(character);
    }
}

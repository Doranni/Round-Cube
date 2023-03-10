using System.Collections.Generic;

public class CharacterEquipment
{
    private readonly Character character;
    public Dictionary<IStorage.StorageNames, IStorage> Storages { get; private set; }
    public Dictionary<SlotsHolder.SlotsHolderNames, SlotsHolder> SlotsHolders { get; private set; }
    public Card SelectedCard { get; private set; }

    public CharacterEquipment(Character character)
    {
        this.character = character;
        Storages = new();
        SlotsHolders = new();
        Storages.Add(IStorage.StorageNames.WeaponSlot, 
            new Slot(IStorage.StorageNames.WeaponSlot, Card.CardsType.Weapon, character));

        Storages.Add(IStorage.StorageNames.ArmorSlot,
            new Slot(IStorage.StorageNames.ArmorSlot, Card.CardsType.Armor, character));
        Storages.Add(IStorage.StorageNames.ShieldSlot,
            new Slot(IStorage.StorageNames.ShieldSlot, Card.CardsType.Shield, character));
        SlotsHolders.Add(SlotsHolder.SlotsHolderNames.Defense, new(SlotsHolder.SlotsHolderNames.Defense, new()
        {
            (Slot)Storages[IStorage.StorageNames.ArmorSlot],
            (Slot)Storages[IStorage.StorageNames.ShieldSlot]
        }));

        Storages.Add(IStorage.StorageNames.OtherSlot1, new Slot(IStorage.StorageNames.OtherSlot1, 
            Card.CardsType.Magic | Card.CardsType.Potion | Card.CardsType.Artifact, character));
        Storages.Add(IStorage.StorageNames.OtherSlot2, new Slot(IStorage.StorageNames.OtherSlot2, 
            Card.CardsType.Magic | Card.CardsType.Potion | Card.CardsType.Artifact, character));
        Storages.Add(IStorage.StorageNames.OtherSlot3, new Slot(IStorage.StorageNames.OtherSlot3, 
            Card.CardsType.Magic | Card.CardsType.Potion | Card.CardsType.Artifact, character));
        SlotsHolders.Add(SlotsHolder.SlotsHolderNames.Other, new(SlotsHolder.SlotsHolderNames.Other, new()
        {
            (Slot)Storages[IStorage.StorageNames.OtherSlot1],
            (Slot)Storages[IStorage.StorageNames.OtherSlot2],
            (Slot)Storages[IStorage.StorageNames.OtherSlot3]
        }));

        Storages.Add(IStorage.StorageNames.Inventory, new Inventory(character));
    }

    public bool AddCard(Card card, IStorage.StorageNames storageName, bool needToSave = true)
    {
        var addRes = Storages[storageName].AddCard(card);
        if (!addRes.success)
        {
            return false;
        }
        if (addRes.releasedCard != null)
        {
            Storages[IStorage.StorageNames.Inventory].AddCard(addRes.releasedCard);
        }
        if (needToSave)
        {
            SavesManager.Instance.UpdateCharacter(character);
        }
        return true;
    }

    public bool RemoveCard(Card card, IStorage.StorageNames storageName)
    {
        var success = Storages[storageName].RemoveCard(card);
        if (success)
        {
            SavesManager.Instance.UpdateCharacter(character);
        }
        return success;
    }

    public bool RemoveCard(Card card)
    {
        foreach (IStorage storage in Storages.Values)
        {
            if (storage.Cards.Find(x => x.InstanceId == card.InstanceId) != null)
            {
                return RemoveCard(card, storage.StorageName);
            }
        }
        return false;
    }

    public bool MoveCard(Card card, IStorage.StorageNames prevStorage, IStorage.StorageNames newStorage)
    {
        if (Storages[prevStorage].RemoveCard(card))
        {
            var addRes = Storages[newStorage].AddCard(card);
            if (!addRes.success)
            {
                var returnRes = Storages[prevStorage].AddCard(card);
                if (!returnRes.success)
                {
                    Storages[IStorage.StorageNames.Inventory].AddCard(card);
                    SavesManager.Instance.UpdateCharacter(character);
                }
                return false;
            }
            if (addRes.releasedCard != null)
            {
                var relesedCardAdd = Storages[prevStorage].AddCard(addRes.releasedCard);
                if (relesedCardAdd.success)
                {
                    SavesManager.Instance.UpdateCharacter(character);
                    return true;
                }
                else
                {
                    Storages[IStorage.StorageNames.Inventory].AddCard(addRes.releasedCard);
                    SavesManager.Instance.UpdateCharacter(character);
                    return true;
                }
            }
            SavesManager.Instance.UpdateCharacter(character);
            return true;
        }
        return false;
    }

    public void SelectCard(Card card)
    {
        var cardToSelect = Storages[card.Storage].Cards.Find(x => x.InstanceId == card.InstanceId);
        if (cardToSelect != null)
        {
            if (SelectedCard != null)
            {
                SelectedCard.SelectCard(false);
                if (SelectedCard.InstanceId == card.InstanceId)
                {
                    SelectedCard = null;
                    return;
                }
            }
            SelectedCard = card;
            card.SelectCard(true);
        }
    }

    public void UnselectCards()
    {
        if (SelectedCard != null)
        {
            SelectCard(SelectedCard);
        }
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterFighting 
{
    private readonly Character character;

    public List<Card> BattleCards { get; private set; }
    public Card SelectedBattleCard { get; private set; }

    public event Action<(int unselectedCardId, int selectedCardId)> SelectedCardChanged;

    public CharacterFighting(Character character)
    {
        this.character = character;
        BattleCards = new();
    }

    public void UpdateCards()
    {
        BattleCards.Clear();
        if (character.Equipment.Storages[IStorage.StorageNames.WeaponSlot].Cards.Count == 1)
        {
            BattleCards.Add(character.Equipment.Storages[IStorage.StorageNames.WeaponSlot].Cards[0]);
        }
        foreach (Slot slot in character.Equipment.SlotsHolders[SlotsHolder.SlotsHolderNames.Armor].Slots)
        {
            if (slot.Cards.Count == 1)
            {
                BattleCards.Add(slot.Cards[0]);
            }
        }
        foreach (Slot slot in character.Equipment.SlotsHolders[SlotsHolder.SlotsHolderNames.Other].Slots)
        {
            if (slot.Cards.Count == 1)
            {
                BattleCards.Add(slot.Cards[0]);
            }
        }
    }

    public void SelectBattleCard(int cardId)
    {
        var card = BattleCards.Find(x => x.InstanceId == cardId);
        if (card != null)
        {
            int unselectedCardId = -1;
            int selectedCardId = -1;
            if (SelectedBattleCard != null)
            {
                unselectedCardId = SelectedBattleCard.InstanceId;
                if (SelectedBattleCard.InstanceId == cardId)
                {
                    SelectedBattleCard = null;
                    SelectedCardChanged?.Invoke((unselectedCardId, selectedCardId));
                    return;
                }
            }
            selectedCardId = cardId;
            SelectedBattleCard = card;
            SelectedCardChanged?.Invoke((unselectedCardId, selectedCardId));
        }
    }

    public void UseBattleCard(Character target)
    {
        if (SelectedBattleCard != null && SelectedBattleCard is IUsable)
        {
            ((IUsable)SelectedBattleCard).Use(target);
            SelectBattleCard(SelectedBattleCard.InstanceId);
            FightingManager.Instance.NextTurn();
        }
    }
}

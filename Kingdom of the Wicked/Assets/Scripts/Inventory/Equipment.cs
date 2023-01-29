using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class Equipment : MonoBehaviour
{
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
        OtherCards = new Dictionary<int, Card>(GameManager.Instance.Equipment_OtherSlotsAmount);
        for (int i = 0; i < GameManager.Instance.Equipment_OtherSlotsAmount; i++)
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
}

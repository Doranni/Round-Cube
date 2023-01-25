using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StatsManager))]
public class Equipment : MonoBehaviour
{
    public CardWeapon WeaponCard { get; private set; }
    public CardArmor ArmorCard { get; private set; }
    public Dictionary<int, CardOther> OtherCards { get; private set; }

    private StatsManager statsManager;

    private void Awake()
    {
        statsManager = GetComponent<StatsManager>();
        OtherCards = new Dictionary<int, CardOther>(GameManager.Instance.OtherCardsEquipSlotsAmount);
    }

    public void EquipWeaponCard(CardWeapon card)
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
    }
    public void EquipArmorCard(CardArmor card)
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
    }
    public void EquipOtherCard(CardOther card, int slotNumber)
    {
        if (OtherCards[slotNumber] != null)
        {
            foreach (StatBonus bonus in OtherCards[slotNumber].StatBonuses)
            {
                statsManager.RemoveBonus(bonus);
            }
        }
        OtherCards[slotNumber] = card;
        foreach (StatBonus bonus in card.StatBonuses)
        {
            statsManager.AddBonus(bonus);
        }
    }
}

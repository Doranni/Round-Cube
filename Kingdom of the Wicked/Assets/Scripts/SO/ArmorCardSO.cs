using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Armor card ", menuName = "Cards/Armor Card")]
public class ArmorCardSO : CardSO
{
    [SerializeField] private int protection;
    [SerializeField] private List<StatBonus> statBonuses;

    public int Protection => protection;
    public List<StatBonus> StatBonuses => statBonuses;
}


using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon card ", menuName = "Cards/Weapon Card")]
public class WeaponCardSO : CardSO
{
    [SerializeField] private List<Effect> effects;
    [SerializeField] private List<StatBonus> statBonuses;

    public List<Effect> Effects => effects;
    public List<StatBonus> StatBonuses => statBonuses;
}


using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Weapon card ", menuName = "Cards/Weapon Card")]
public class WeaponCardSO : CardSO
{
    public List<Effect> effects;
    public List<StatBonus> statBonuses;
}


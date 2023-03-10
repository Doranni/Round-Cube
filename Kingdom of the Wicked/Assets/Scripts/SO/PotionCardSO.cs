using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Potion card ", menuName = "Cards/Potion Card")]
public class PotionCardSO : CardSO
{
    [SerializeField] private List<Effect> effects;
    [SerializeField] private int charges;

    public List<Effect> Effects => effects;
    public int Charges => charges;
}


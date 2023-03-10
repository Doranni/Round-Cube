using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic card ", menuName = "Cards/Magic Card")]
public class MagicCardSO : CardSO
{
    [SerializeField] private List<Effect> effects;
    [SerializeField] private int charges;

    public List<Effect> Effects => effects;
    public int Charges => charges;
}


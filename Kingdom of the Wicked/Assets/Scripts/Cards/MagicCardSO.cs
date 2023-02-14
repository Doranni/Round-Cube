using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Magic card ", menuName = "Cards/Magic Card")]
public class MagicCardSO : CardSO
{
    public List<Effect> effects;
}


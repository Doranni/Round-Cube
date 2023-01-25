using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class StatBonus
{
    [SerializeField] private Stat.StatId statTypeId;
    [SerializeField] private int value;
    private int bonusId;

    public Stat.StatId StatTypeId => statTypeId;
    public int Value => value;  
    public int BonusId => bonusId;

    public void SetId(int id)
    {
        bonusId = id;
    }
}

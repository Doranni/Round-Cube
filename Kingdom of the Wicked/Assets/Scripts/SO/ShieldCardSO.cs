using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Shield card ", menuName = "Cards/Shield Card")]
public class ShieldCardSO : CardSO
{
    [SerializeField] private int blockCharges;
    [SerializeField][Range (0, 1)] private float blockChanse;
    [SerializeField] private List<StatBonus> statBonuses;

    public int BlockCharges => blockCharges;
    public float BlockChanse => blockChanse;
    public List<StatBonus> StatBonuses => statBonuses;
}


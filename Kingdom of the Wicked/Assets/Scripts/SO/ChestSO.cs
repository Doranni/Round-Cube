using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chest ", menuName = "Chest")]
public class ChestSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private int[] cardsId;

    public int Id => id;
    public int[] CardsId => cardsId;
}

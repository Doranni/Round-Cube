using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Chest ", menuName = "Chest")]
public class ChestSO : ScriptableObject
{
    public int id;
    public int[] cardsId;
}

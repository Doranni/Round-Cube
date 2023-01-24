using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Card : MonoBehaviour
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public List<StatBonus> StatBonuses { get; private set; }
    public int Duration { get; private set; }

    
}

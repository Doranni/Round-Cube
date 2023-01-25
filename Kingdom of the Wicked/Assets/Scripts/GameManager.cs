using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] [Range(1, 5)] private int otherCardsEquipSlotsAmount;
    public int Equipment_OtherSlotsAmount => otherCardsEquipSlotsAmount;

    private int id = 0;

    public int GetID()
    {
        return id++;
    }


}

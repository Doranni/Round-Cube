using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        inMenu,
        active
    }

    [SerializeField] [Range(1, 5)] private int otherCardsEquipSlotsAmount;

    public int Equipment_OtherSlotsAmount => otherCardsEquipSlotsAmount;

    public GameState State { get; private set; }

    private int id = 0;

    private void Start()
    {
        EquipmentUI.Instance.OnToggleOpenInvemtory += ToggleOpenInvemtory;
        State = GameState.active;
    }

    private void ToggleOpenInvemtory(bool isInventoryOpen)
    {
        if (isInventoryOpen)
        {
            SetState(GameState.inMenu);
        }
        else
        {
            SetState(GameState.active);
        }
    }

    private void SetState(GameState state)
    {
        State = state;
    }

    public int GetID()
    {
        return id++;
    }


}

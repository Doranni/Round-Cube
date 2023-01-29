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

    [SerializeField] [Range(1, 5)] private int otherSlotCapacity = 3;
    [SerializeField][Range(1, 100)] private int inventoryCapacity = 50;

    public int Equipment_OtherSlotCapacity => otherSlotCapacity;
    public int Equipment_InventoryCapacity => inventoryCapacity;

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

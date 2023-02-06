using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

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

    [SerializeField] private Vector2 cardSize_small = new(80, 120),
        cardSize_big = new(120, 160);
    [SerializeField] private float inventoryCardMargin = 20;
    [SerializeField] private Vector2 dragRangeMin = new(20, 20), dragRangeMax = new(1820, 940);
    private VisualTreeAsset cardAsset;

    public Vector2 CardSize_small => cardSize_small;
    public Vector2 CardSize_big => cardSize_big;
    public float InventoryCardMargin => inventoryCardMargin;
    public Vector2 DragRangeMin => dragRangeMin;
    public Vector2 DragRangeMax => dragRangeMax;
    public VisualTreeAsset CardAsset => cardAsset;

    public GameState State { get; private set; }

    private int id = 0;

    public override void Awake()
    {
        base.Awake();
        cardAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;
    }

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

    public Vector2 GetRequiredCardSize(IStorage.StorageNames storageName)
    {
        switch (storageName)
        {
            case IStorage.StorageNames.inventory:
            case IStorage.StorageNames.storage:
                {
                    return cardSize_big;
                }
            default:
                {
                    return cardSize_small;
                }
        }
    }


}

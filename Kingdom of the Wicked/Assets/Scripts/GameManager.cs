using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private float plMovementHeight = 4;
    public float PlMovementHeight => plMovementHeight;

    [SerializeField] [Range(1, 5)] private int otherSlotCapacity = 3;
    [SerializeField][Range(1, 100)] private int inventoryCapacity = 50;

    public int Equipment_OtherSlotCapacity => otherSlotCapacity;
    public int Equipment_InventoryCapacity => inventoryCapacity;

    [SerializeField] private Vector2 cardSize_slot = new(80, 120),
        cardSize_inventory = new(120, 160), cardSize_dragging = new(120, 160);
    [SerializeField] private float inventoryCardMargin = 20;
    private VisualTreeAsset cardAsset, slotsHolderAsset;

    public Vector2 CardSize_slot => cardSize_slot;
    public Vector2 CardSize_inventory => cardSize_inventory;
    public Vector2 CardSize_dragging => cardSize_dragging;
    public float InventoryCardMargin => inventoryCardMargin;
    public VisualTreeAsset CardAsset => cardAsset;
    public VisualTreeAsset SlotsHolderAsset => slotsHolderAsset;

    public bool GameIsActive { get; set; }

    private int id = 0;

    protected override void Awake()
    {
        base.Awake();
        cardAsset = EditorGUIUtility.Load("Assets/UI/VECard.uxml") as VisualTreeAsset;
        slotsHolderAsset = EditorGUIUtility.Load("Assets/UI/VESlotHolder.uxml") as VisualTreeAsset;
    }

    private void Start()
    {
        GameIsActive = true;
    }

    public void StartFight(int mapNode_index)
    {
        LoadSceneManager.Instance.LoadScene(LoadSceneManager.Scenes.Fighting);
    }

    public void EndFight()
    {
        LoadSceneManager.Instance.LoadScene(LoadSceneManager.Scenes.Board);
    }

    public int GetID()
    {
        return id++;
    }

    public Vector2 GetRequiredCardSize(IStorage.StorageNames storageName)
    {
        switch (storageName)
        {
            case IStorage.StorageNames.Inventory:
            case IStorage.StorageNames.Storage:
                {
                    return cardSize_inventory;
                }
            default:
                {
                    return cardSize_slot;
                }
        }
    }
}

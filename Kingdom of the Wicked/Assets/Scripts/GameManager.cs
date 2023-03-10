using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : Singleton<GameManager>
{
    public enum GameState
    {
        Loading,
        Menu,
        BoardActive,
        BoardInventory,
        BoardChest,
        BoardMenu,
        FightingActive,
        FightingMenu
    }

    public GameState State { get; private set; }

    [SerializeField] private float plMovementHeight = 4;
    public float PlMovementHeight => plMovementHeight;

    [SerializeField] [Range(1, 5)] private int otherSlotCapacity = 3;
    [SerializeField][Range(1, 100)] private int inventoryCapacity = 50;

    public int Equipment_OtherSlotCapacity => otherSlotCapacity;
    public int Equipment_InventoryCapacity => inventoryCapacity;

    [SerializeField] private Vector2 cardSize_regular = new(80, 120),
        cardSize_selected = new(88, 132);
    [SerializeField] private float inventoryCardMargin = 20;
    private VisualTreeAsset cardAsset, slotsHolderAsset, characterPanelAsset;

    public Vector2 CardSize_regular => cardSize_regular;
    public Vector2 CardSize_selected => cardSize_selected;
    public float InventoryCardMargin => inventoryCardMargin;
    public VisualTreeAsset CardAsset => cardAsset;
    public VisualTreeAsset SlotsHolderAsset => slotsHolderAsset;
    public VisualTreeAsset CharacterPanelAsset => characterPanelAsset;

    private int id = 0;

    protected override void Awake()
    {
        base.Awake();
        cardAsset = EditorGUIUtility.Load("Assets/UI/VECard.uxml") as VisualTreeAsset;
        slotsHolderAsset = EditorGUIUtility.Load("Assets/UI/VESlotHolder.uxml") as VisualTreeAsset;
        characterPanelAsset = EditorGUIUtility.Load("Assets/UI/VECharacter.uxml") as VisualTreeAsset;
    }

    private void Start()
    {
        InputManager.Instance.UIEscape_performed += _ => GameUIEscape_performed();
    }

    private void GameUIEscape_performed()
    {
        switch (State)
        {
            case GameState.BoardInventory:
                {
                    BoardSceneUI.Instance.OpenInventory(false);
                    break;
                }
            case GameState.BoardChest:
                {
                    // Close the chest
                    break;
                }
            case GameState.BoardActive:
            case GameState.FightingActive:
                {
                    MainSceneUI.Instance.OpenMenuScreen(true);
                    break;
                }
            case GameState.BoardMenu:
            case GameState.FightingMenu:
                {
                    MainSceneUI.Instance.OpenMenuScreen(false);
                    break;
                }
        }
    }

    public void OpenChest(ChestController chest)
    {
        if (State == GameState.BoardInventory)
        {
            BoardSceneUI.Instance.OpenInventory(false);
            State = GameState.BoardChest;
            StartCoroutine(OpenChestRoutine(chest));
        }
        else if (State == GameState.BoardActive)
        {
            State = GameState.BoardChest;
            StartCoroutine(OpenChestRoutine(chest));
        }
    }

    private IEnumerator OpenChestRoutine(ChestController chest)
    {
        BoardCameraController.Instance.SetFocusTarget(chest.transform);
        yield return new WaitForSeconds(0.8f);
        chest.StartOpenAnimation();
    }

    public void StartFight()
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

    public void UpdateState()
    {
        switch (LoadSceneManager.Instance.State)
        {
            case LoadSceneManager.LoadState.Loading:
                {
                    State = GameState.Loading;
                    break;
                }
            case LoadSceneManager.LoadState.Menu:
                {
                    State = GameState.Menu;
                    break;
                }
            case LoadSceneManager.LoadState.Board:
                {
                    if (MainSceneUI.Instance.MenuScreenIsOpen)
                    {
                        State = GameState.BoardMenu;
                    }
                    else
                    {
                        switch (BoardSceneUI.Instance.State)
                        {
                            case BoardSceneUI.BoardUIState.Closed:
                                {
                                    State = GameState.BoardActive;
                                    break;
                                }
                            case BoardSceneUI.BoardUIState.Inventory:
                                {
                                    State = GameState.BoardInventory;
                                    break;
                                }
                            case BoardSceneUI.BoardUIState.Chest:
                                {
                                    State = GameState.BoardChest;
                                    break;
                                }
                        }
                    }
                    break;
                }
            case LoadSceneManager.LoadState.Fighting:
                {
                    if (MainSceneUI.Instance.MenuScreenIsOpen)
                    {
                        State = GameState.FightingMenu;
                    }
                    else
                    {
                        State = GameState.FightingActive;
                    }
                    break;
                }
        }
    }
}

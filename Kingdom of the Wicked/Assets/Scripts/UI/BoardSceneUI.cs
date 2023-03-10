using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class BoardSceneUI : Singleton<BoardSceneUI>
{
    public enum BoardUIState
    {
        Closed,
        Inventory,
        Chest
    }

    [SerializeField] private Character player;

    public BoardUIState State { get; private set; }

    private List<StorageVE> storages;
    private Dictionary<SlotsHolder.SlotsHolderNames, SlotsHolderVE> slotsHolders;

    private HealthBarVE plHealthBar;

    private StorageVE weaponSlot, inventory;
    private VisualElement inventoryButton, inventoryScreen;

    private VisualElement chestScreen;
    private RewardVE chest;
    private Button chestTakeAllButton;

    private VisualElement dragCardPanel;

    const string k_plHealthBar = "PlayerHP";
    const string k_slot_weapon = "PlSlot_Weapon";
    const string k_slotsHolder_armor = "PlSlotsHolder_Armor";
    const string k_slotsHolder_other = "PlSlotsHolder_Other";

    const string k_inventoryButton = "InventoryButton";
    const string k_inventoryScreen = "Inventory";
    const string k_inventoryContent = "InventoryContent";

    const string k_chestScreen = "Chest";
    const string k_chest = "ChestContent";
    const string k_chestTakeAllButton = "ButtonChestTakeAll";

    const string k_dragCardPanel = "DragCardPanel";

    protected override void Awake()
    {
        base.Awake();

        State = BoardUIState.Closed;

        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        plHealthBar = rootElement.Q<HealthBarVE>(k_plHealthBar);

        weaponSlot = rootElement.Q<SlotVE>(k_slot_weapon);
        inventoryButton = rootElement.Q(k_inventoryButton);
        inventoryScreen = rootElement.Q(k_inventoryScreen);
        inventory = rootElement.Q<InventoryVE>(k_inventoryContent);

        chestScreen = rootElement.Q(k_chestScreen);
        chest = rootElement.Q<RewardVE>(k_chest);
        chestTakeAllButton = rootElement.Q<Button>(k_chestTakeAllButton);

        dragCardPanel = rootElement.Q(k_dragCardPanel);

        storages = new()
        {
            weaponSlot,
            inventory
        };

        slotsHolders = new()
        {
            { SlotsHolder.SlotsHolderNames.Defense, rootElement.Q<SlotsHolderVE>(k_slotsHolder_armor) },
            { SlotsHolder.SlotsHolderNames.Other, rootElement.Q<SlotsHolderVE>(k_slotsHolder_other) }
        };

        var size = GameManager.Instance.CardSize_regular;
        inventoryButton.style.width = size.x;
        inventoryButton.style.height = size.y;

        chestScreen.style.display = DisplayStyle.None;
    }

    private void Start()
    {
        plHealthBar.Init(player.Stats.ChHealth);
        weaponSlot.Init(player.Equipment.Storages[IStorage.StorageNames.WeaponSlot]);
        inventory.Init(player.Equipment.Storages[IStorage.StorageNames.Inventory]);
        inventory.SetIsActive(false);

        foreach (KeyValuePair<SlotsHolder.SlotsHolderNames, SlotsHolderVE> slotHolder in slotsHolders)
        {
            slotHolder.Value.Init(player.Equipment.SlotsHolders[slotHolder.Key]);
            foreach ((SlotVE closedSlot, SlotVE openSlot) in slotHolder.Value.Slots)
            {
                storages.Add(closedSlot);
                storages.Add(openSlot);
            }
        }

        inventoryButton.RegisterCallback<ClickEvent>(_ => InventoryButtonClicked()) ;
        player.Equipment.Storages[IStorage.StorageNames.Inventory].CardsChanged += DisplayInventoryButton;

        DisplayInventoryButton();
        DisplayInventory();

        chestTakeAllButton.RegisterCallback<ClickEvent>(_ => CloseChest());
    }

    public void OpenChest(Reward chest)
    {
        this.chest.Init(chest);
        chestScreen.style.display = DisplayStyle.Flex;
        State = BoardUIState.Chest;
    }

    public void CloseChest()
    {
        StartCoroutine(CloseChestRoutine());
    }

    private IEnumerator CloseChestRoutine()
    {
        for (int i = 0; i < chest.Storage.Cards.Count; i++)
        {
            player.Equipment.AddCard(chest.Storage.Cards[i], IStorage.StorageNames.Inventory);
        }
        chestScreen.style.display = DisplayStyle.None;
        yield return new WaitForSeconds(0.2f);
        BoardCameraController.Instance.UnsetFocusTarget();
        chest.Reset();
        State = BoardUIState.Closed;
        GameManager.Instance.UpdateState();
    }

    private void DisplayInventoryButton()
    {
        if (player.Equipment.Storages[IStorage.StorageNames.Inventory].Cards.Count == 0)
        {
            inventoryButton.style.display = DisplayStyle.None;
            if (inventory.IsActive)
            {
                ToggleOpenInvemtory();
            }
        }
        else
        {
            inventoryButton.style.display = DisplayStyle.Flex;
        }
    }

    private void DisplayInventory()
    {
        if (inventory.IsActive)
        {
            inventoryScreen.style.display = DisplayStyle.Flex;
        }
        else
        {
            inventoryScreen.style.display = DisplayStyle.None;
        }
    }

    private void InventoryButtonClicked()
    {
        if (State != BoardUIState.Chest)
        {
            ToggleOpenInvemtory();
        }
    }

    private void ToggleOpenInvemtory()
    {
        inventory.SetIsActive(!inventory.IsActive);
        DisplayInventory();
        if (inventory.IsActive)
        {
            State = BoardUIState.Inventory;
        }
        else
        {
            State = BoardUIState.Closed;
        }
        GameManager.Instance.UpdateState();
    }

    public void OpenInventory(bool value)
    {
        if (value != inventory.IsActive)
        {
            ToggleOpenInvemtory();
        }
    }

    public void OpenSlotsHolders(Card card, bool value)
    {
        switch (card.CardType)
        {
            case Card.CardsType.Armor:
            case Card.CardsType.Shield:
                {
                    if (slotsHolders[SlotsHolder.SlotsHolderNames.Defense].IsOpen != value)
                    {
                        slotsHolders[SlotsHolder.SlotsHolderNames.Defense].ToggleSlotPanel(value);
                    }
                    break;
                }
            case Card.CardsType.Magic:
            case Card.CardsType.Potion:
            case Card.CardsType.Artifact:
                {
                    if (slotsHolders[SlotsHolder.SlotsHolderNames.Other].IsOpen != value)
                    {
                        slotsHolders[SlotsHolder.SlotsHolderNames.Other].ToggleSlotPanel(value);
                    }
                    break;
                }
        }
    }

    public List<StorageVE> GetAvailableStorages()
    {
        List<StorageVE> res = new();
        foreach (StorageVE storage in storages)
        {
            if (storage.IsActive)
            {
                res.Add(storage);
            }
        }
        return res;
    }

    public void CardWasMoved(Card card, IStorage.StorageNames prevStorage, 
        List<IStorage.StorageNames> newStorages)
    {
        if (newStorages.Count == 0 &&
            (prevStorage != IStorage.StorageNames.Inventory && prevStorage != IStorage.StorageNames.Reward))
        {
            player.Equipment.MoveCard(card, prevStorage, IStorage.StorageNames.Inventory);
            OpenSlotsHolders(card, false);
            return;
        }
        int lastEquippedSlot = -1;
        for(int i = 0; i < newStorages.Count; i++)
        {
            if ((prevStorage != newStorages[i]) && Card.ComperaCardTypesFlags(card.CardType, 
                player.Equipment.Storages[newStorages[i]].CardTypes))
            {
                if (player.Equipment.Storages[newStorages[i]].IsFull)
                {
                    lastEquippedSlot = i;
                    continue;
                }
                lastEquippedSlot = -1;
                player.Equipment.MoveCard(card, prevStorage, newStorages[i]);
                return;
            }
        }
        if (lastEquippedSlot != -1)
        {
            player.Equipment.MoveCard(card, prevStorage, newStorages[lastEquippedSlot]);
        }
        else
        {
            if (prevStorage == IStorage.StorageNames.Inventory)
            {
                OpenSlotsHolders(card, false);
            }
        }
    }
}

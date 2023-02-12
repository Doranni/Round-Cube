using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EquipmentUI : Singleton<EquipmentUI>
{
    [SerializeField] private CharacterEquipment plEquipment;

    private List<StorageVE> storages;
    private Dictionary<SlotsHolder.SlotsHolderNames, SlotsHolderVE> slotsHolders;

    private VisualElement plEquipmentScreen;
    private VisualElement inventoryButton, inventoryScreen;
    private VisualElement dragCardPanel;
    private StorageVE weaponSlot, inventory;

    const string k_equipmentScreen = "PlayerEquipment";

    const string k_slotsHolder_armor = "SlotsHolder_Armor";
    const string k_slotsHolder_other = "SlotsHolder_Other";

    const string k_slot_weapon = "Slot_Weapon";

    const string k_inventoryButton = "InventoryButton";
    const string k_inventoryScreen = "Inventory";
    const string k_inventoryContent = "InventoryContent";

    const string k_dragCardPanel = "DragCardPanel";

    public event Action<bool> OpenInvemtoryToggled;

    public override void Awake()
    {
        base.Awake();

        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        plEquipmentScreen = rootElement.Q(k_equipmentScreen);
        inventoryButton = rootElement.Q(k_inventoryButton);
        inventoryScreen = rootElement.Q(k_inventoryScreen);
        dragCardPanel = rootElement.Q(k_dragCardPanel);
        weaponSlot = rootElement.Q<SlotVE>(k_slot_weapon);
        inventory = rootElement.Q<InventoryVE>(k_inventoryContent);

        storages = new();
        storages.Add(weaponSlot);
        storages.Add(inventory);

        slotsHolders = new();
        slotsHolders.Add(SlotsHolder.SlotsHolderNames.Armor, rootElement.Q<SlotsHolderVE>(k_slotsHolder_armor));
        slotsHolders.Add(SlotsHolder.SlotsHolderNames.Other, rootElement.Q<SlotsHolderVE>(k_slotsHolder_other));

        var size = GameManager.Instance.CardSize_slot;
        inventoryButton.style.width = size.x;
        inventoryButton.style.height = size.y;
    }

    private void Start()
    {
        weaponSlot.Init(plEquipment.Storages[IStorage.StorageNames.WeaponSlot]);
        inventory.Init(plEquipment.Storages[IStorage.StorageNames.Inventory]);
        inventory.SetIsActive(false);

        foreach (KeyValuePair<SlotsHolder.SlotsHolderNames, SlotsHolderVE> slotHolder in slotsHolders)
        {
            slotHolder.Value.Init(plEquipment.SlotsHolders[slotHolder.Key]);
            foreach ((SlotVE closedSlot, SlotVE openSlot) in slotHolder.Value.Slots)
            {
                storages.Add(closedSlot);
                storages.Add(openSlot);
            }
        }

        DragAndDropController.Instance.Init(dragCardPanel);

        inventoryButton.RegisterCallback<ClickEvent>(_ => ToggleOpenInvemtory()) ;
        InputManager.Instance.UIEscape_performed += _ => GameUIEscape_performed();
        plEquipment.Storages[IStorage.StorageNames.Inventory].CardsChanged += DisplayInventoryButton;

        foreach (StorageVE storage in storages)
        {
            storage.Update();
        }
        DisplayInventoryButton();
        DisplayInventory();
    }

    private void GameUIEscape_performed()
    {
        if (inventory.IsActive)
        {
            ToggleOpenInvemtory();
        }
    }

    private void DisplayInventoryButton()
    {
        if (plEquipment.Storages[IStorage.StorageNames.Inventory].Cards.Count == 0)
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

    private void ToggleOpenInvemtory()
    {
        inventory.SetIsActive(!inventory.IsActive);
        DisplayInventory();
        OpenInvemtoryToggled?.Invoke(inventory.IsActive);
    }

    public void OpenSlotsHolders(Card card, bool value)
    {
        switch (card.CardType)
        {
            case Card.CardsType.Armor:
            case Card.CardsType.Shield:
                {
                    if (slotsHolders[SlotsHolder.SlotsHolderNames.Armor].IsOpen != value)
                    {
                        slotsHolders[SlotsHolder.SlotsHolderNames.Armor].ToggleSlotPanel(value);
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

    public List<StorageVE> GetAvailableStorages(Card.CardsType type)
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
            (prevStorage != IStorage.StorageNames.Inventory && prevStorage != IStorage.StorageNames.Storage))
        {
            plEquipment.MoveCard(card, prevStorage, IStorage.StorageNames.Inventory);
            OpenSlotsHolders(card, false);
            return;
        }
        int lastEquippedSlot = -1;
        for(int i = 0; i < newStorages.Count; i++)
        {
            if ((prevStorage != newStorages[i]) && Card.ComperaCardTypesFlags(card.CardType, 
                plEquipment.Storages[newStorages[i]].CardTypes))
            {
                if (plEquipment.Storages[newStorages[i]].IsFull)
                {
                    lastEquippedSlot = i;
                    continue;
                }
                lastEquippedSlot = -1;
                plEquipment.MoveCard(card, prevStorage, newStorages[i], false);
                return;
            }
        }
        if (lastEquippedSlot != -1)
        {
            plEquipment.MoveCard(card, prevStorage, newStorages[lastEquippedSlot], false);
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

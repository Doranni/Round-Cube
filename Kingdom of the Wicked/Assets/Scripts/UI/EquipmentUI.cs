using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EquipmentUI : Singleton<EquipmentUI>
{
    [SerializeField] private int inventoryRowCardsCapacity = 10;

    [SerializeField] private Equipment plEquipment;

    private VisualElement plEquipmentScreen;
    private VisualElement inventoryButton, inventoryScreen;
    private VisualElement plInventoryCardsPanel, dragCardPanel;

    public Dictionary<IStorage.StorageNames, StorageUI> storages = new();

    const string k_equipmentScreen = "PlayerEquipment";
    const string k_slotWeapon = "SlopWeapon";
    const string k_slotArmor = "SlotArmor";
    const string k_slotOther = "SlotOther";
    const string k_inventoryButton = "InventoryButton";
    const string k_inventoryScreen = "Inventory";
    const string k_inventoryContent = "InventoryContent";
    const string k_dragCardPanel = "DragCardPanel";

    private VisualTreeAsset cardAsset;

    public event Action<bool> OnToggleOpenInvemtory;

    public override void Awake()
    {
        base.Awake();

        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        plEquipmentScreen = rootElement.Q(k_equipmentScreen);
        inventoryButton = rootElement.Q(k_inventoryButton);
        inventoryScreen = rootElement.Q(k_inventoryScreen);
        plInventoryCardsPanel = rootElement.Q(k_inventoryContent);
        dragCardPanel = rootElement.Q(k_dragCardPanel);

        storages.Add(IStorage.StorageNames.weaponSlot, new(plEquipment.Storages[IStorage.StorageNames.weaponSlot],
            true, rootElement.Q(k_slotWeapon), rootElement.Q(k_slotWeapon)));
        storages.Add(IStorage.StorageNames.armorSlot, new(plEquipment.Storages[IStorage.StorageNames.armorSlot], 
            true, rootElement.Q(k_slotArmor), rootElement.Q(k_slotArmor)));
        storages.Add(IStorage.StorageNames.otherSlot, new(plEquipment.Storages[IStorage.StorageNames.otherSlot], 
            true, rootElement.Q(k_slotOther), rootElement.Q(k_slotOther)));
        storages.Add(IStorage.StorageNames.inventory, new(plEquipment.Storages[IStorage.StorageNames.inventory], 
            false, plInventoryCardsPanel, plInventoryCardsPanel));

        cardAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;

        UIManager.Instance.SetSize(storages[IStorage.StorageNames.weaponSlot].StorageVE,
            IStorage.StorageNames.weaponSlot);
        UIManager.Instance.SetSize(storages[IStorage.StorageNames.armorSlot].StorageVE,
            IStorage.StorageNames.armorSlot);
        UIManager.Instance.SetSize(storages[IStorage.StorageNames.otherSlot].StorageVE,
            IStorage.StorageNames.otherSlot);
        UIManager.Instance.SetSize(inventoryButton, IStorage.StorageNames.weaponSlot);
    }

    private void Start()
    {
        DragAndDropController.Instance.SetCardToDrag(cardAsset.CloneTree(), dragCardPanel);

        plEquipment.OnStorageChanged += DisplayCards;

        inventoryButton.RegisterCallback<ClickEvent>(_ => ToggleOpenInvemtory()) ;
        InputManager.Instance.OnUIEscape_performed += _ => GameUIEscape_performed();

        DisplayCards(IStorage.StorageNames.weaponSlot);
        DisplayCards(IStorage.StorageNames.armorSlot);
        DisplayCards(IStorage.StorageNames.otherSlot);
        DisplayInventoryButton();
        DisplayCards(IStorage.StorageNames.inventory);
        DisplayInventory();
    }

    private void GameUIEscape_performed()
    {
        if (storages[IStorage.StorageNames.inventory].IsActive)
        {
            ToggleOpenInvemtory();
        }
    }

    private void DisplayInventoryButton()
    {
        if (plEquipment.Storages[IStorage.StorageNames.inventory].Cards.Count == 0)
        {
            inventoryButton.style.display = DisplayStyle.None;
            if (storages[IStorage.StorageNames.inventory].IsActive)
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
        if (storages[IStorage.StorageNames.inventory].IsActive)
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
        storages[IStorage.StorageNames.inventory].SetIsActive(!storages[IStorage.StorageNames.inventory].IsActive);
        DisplayInventory();
        OnToggleOpenInvemtory?.Invoke(storages[IStorage.StorageNames.inventory].IsActive);
    }

    private void DisplayCards(IStorage.StorageNames storageName)
    {
        storages[storageName].Update();
        for (int i = 0; i < storages[storageName].Cards.Count; i++)
        {
            VisualElement cardUI = cardAsset.CloneTree();
            storages[storageName].SetCardVE(i, cardUI);
            UIManager.Instance.StyleCard(storageName, cardUI, storages[storageName].Cards[i].card);
            cardUI.RegisterCallback<PointerDownEvent, (VisualElement, Card, IStorage.StorageNames)>
                (DragAndDropController.Instance.AddTarget, 
                (cardUI, storages[storageName].Cards[i].card, storageName));
        }
        //if (storageName == IStorage.StorageNames.otherSlot && storages[storageName].Storage.ActiveSlot != -1)
        //{

            //    storages[storageName].Cards[storages[storageName].Storage.ActiveSlot].cardVE.BringToFront();
            //}
        if (storageName == IStorage.StorageNames.inventory)
        {
            DisplayInventoryButton();
        }
    }

    public List<(IStorage.StorageNames storageName, VisualElement storageUI)> GetAvailableStorages(
        Card.CardsType type)
    {
        List<(IStorage.StorageNames, VisualElement)> res = new();
        IStorage.AvailableCardsTypes flags = new();
        switch (type)
        {
            case Card.CardsType.Weapon:
                {
                    flags = IStorage.AvailableCardsTypes.weapon;
                    break;
                }
            case Card.CardsType.Armor:
                {
                    flags = IStorage.AvailableCardsTypes.armor;
                    break;
                }
            case Card.CardsType.Other:
                {
                    flags = IStorage.AvailableCardsTypes.other;
                    break;
                }
        }
        foreach (KeyValuePair<IStorage.StorageNames, StorageUI> storage in storages)
        {
            if (storage.Value.IsActive && storage.Value.Storage.CardsTypes.HasFlag(flags))
            {
                res.Add((storage.Key, storage.Value.StorageVE));
            }
        }
        return res;
    }

    public void CardWasMoved(VisualElement cardVE, Card card, IStorage.StorageNames prevStorage, 
        IStorage.StorageNames newStorage)
    {
        if (prevStorage != newStorage)
        {
            plEquipment.MoveCard(card, prevStorage, newStorage);
        }
        Debug.Log($"Card was moved from {prevStorage} to {newStorage}");
    }
}

public class StorageUI
{
    public IStorage Storage { get; private set; }
    public bool IsActive { get; private set; }
    public VisualElement StorageVE { get; private set; }
    public VisualElement CardPanel { get; private set; }
    public List<(Card card, VisualElement cardVE)> Cards { get; private set; }

    public StorageUI(IStorage storage, bool isActive, VisualElement storageVE, VisualElement cardPanel)
    {
        Storage = storage;
        IsActive = isActive;
        StorageVE = storageVE;
        CardPanel = cardPanel;
        if (Storage.Capacity != -1)
        {
            Cards = new(Storage.Capacity);
        }
        else
        {
            Cards = new();
        }
        Update();
    }

    public void Update()
    {
        for (int i = 0; i < Cards.Count; i++)
        {
            CardPanel.Remove(Cards[i].cardVE);
        }
        Cards.Clear();
        for (int i = 0; i < Storage.Cards.Count; i++)
        {
            Cards.Add((Storage.Cards[i], null));
        }
    }

    public void SetCardVE(int index, VisualElement cardVE)
    {
        if (index >= 0 && index < Cards.Count)
        {
            Cards[index] = (Cards[index].card, cardVE);
            CardPanel.Add(cardVE);
        }
    }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
}

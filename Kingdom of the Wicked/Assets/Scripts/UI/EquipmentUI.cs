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

    public Dictionary<Storage.StorageNames, StorageUI> storages = new();

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

        storages.Add(Storage.StorageNames.weaponSlot, new(plEquipment.Storages[Storage.StorageNames.weaponSlot],
            true, rootElement.Q(k_slotWeapon), rootElement.Q(k_slotWeapon)));
        storages.Add(Storage.StorageNames.armorSlot, new(plEquipment.Storages[Storage.StorageNames.armorSlot], 
            true, rootElement.Q(k_slotArmor), rootElement.Q(k_slotArmor)));
        storages.Add(Storage.StorageNames.otherSlot, new(plEquipment.Storages[Storage.StorageNames.otherSlot], 
            true, rootElement.Q(k_slotOther), rootElement.Q(k_slotOther)));
        storages.Add(Storage.StorageNames.inventory, new(plEquipment.Storages[Storage.StorageNames.inventory], 
            false, plInventoryCardsPanel, plInventoryCardsPanel));

        cardAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;

        UIManager.Instance.SetSize(storages[Storage.StorageNames.weaponSlot].StorageVE, 
            Storage.StorageNames.weaponSlot);
        UIManager.Instance.SetSize(storages[Storage.StorageNames.armorSlot].StorageVE, 
            Storage.StorageNames.armorSlot);
        UIManager.Instance.SetSize(storages[Storage.StorageNames.otherSlot].StorageVE, 
            Storage.StorageNames.otherSlot);
        UIManager.Instance.SetSize(inventoryButton, Storage.StorageNames.weaponSlot);
    }

    private void Start()
    {
        DragAndDropController.Instance.SetCardToDrag(cardAsset.CloneTree(), dragCardPanel);

        plEquipment.Storages[Storage.StorageNames.weaponSlot].OnStorageChanged += delegate
        { DisplayCards(Storage.StorageNames.weaponSlot); };
        plEquipment.Storages[Storage.StorageNames.armorSlot].OnStorageChanged += delegate
        { DisplayCards(Storage.StorageNames.armorSlot); };
        plEquipment.Storages[Storage.StorageNames.otherSlot].OnStorageChanged += delegate
        { DisplayCards(Storage.StorageNames.otherSlot); };
        plEquipment.Storages[Storage.StorageNames.inventory].OnStorageChanged += delegate
        { DisplayCards(Storage.StorageNames.inventory); DisplayInventoryButton(); };

        inventoryButton.RegisterCallback<ClickEvent>(_ => ToggleOpenInvemtory()) ;
        InputManager.Instance.OnUIEscape_performed += _ => GameUIEscape_performed();

        DisplayCards(Storage.StorageNames.weaponSlot);
        DisplayCards(Storage.StorageNames.armorSlot);
        DisplayCards(Storage.StorageNames.otherSlot);
        DisplayInventoryButton();
        DisplayCards(Storage.StorageNames.inventory);
        DisplayInventory();
    }

    private void GameUIEscape_performed()
    {
        if (storages[Storage.StorageNames.inventory].IsActive)
        {
            ToggleOpenInvemtory();
        }
    }

    private void DisplayInventoryButton()
    {
        if (plEquipment.Storages[Storage.StorageNames.inventory].Cards.Count == 0)
        {
            inventoryButton.style.display = DisplayStyle.None;
            if (storages[Storage.StorageNames.inventory].IsActive)
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
        if (storages[Storage.StorageNames.inventory].IsActive)
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
        storages[Storage.StorageNames.inventory].SetIsActive(!storages[Storage.StorageNames.inventory].IsActive);
        DisplayInventory();
        OnToggleOpenInvemtory?.Invoke(storages[Storage.StorageNames.inventory].IsActive);
    }

    private void DisplayCards(Storage.StorageNames storageName)
    {
        storages[storageName].Update();
        for (int i = 0; i < storages[storageName].Cards.Count; i++)
        {
            VisualElement cardUI = cardAsset.CloneTree();
            storages[storageName].SetCardVE(i, cardUI);
            UIManager.Instance.StyleCard(storageName, cardUI, storages[storageName].Cards[i].card);
            cardUI.RegisterCallback<PointerDownEvent, (VisualElement, Card, Storage.StorageNames)>
                (DragAndDropController.Instance.AddTarget, 
                (cardUI, storages[storageName].Cards[i].card, storageName));
        }
        if (storageName == Storage.StorageNames.otherSlot && storages[storageName].Storage.ActiveSlot != -1)
        {

            storages[storageName].Cards[storages[storageName].Storage.ActiveSlot].cardVE.BringToFront();
        }
        else if (storageName == Storage.StorageNames.inventory)
        {

        }
    }

    public List<(Storage.StorageNames storageName, VisualElement storageUI)> GetAvailableStorages(
        Card.CardsType type)
    {
        List<(Storage.StorageNames, VisualElement)> res = new();
        Storage.AvailableCardsTypes flags = new();
        switch (type)
        {
            case Card.CardsType.Weapon:
                {
                    flags = Storage.AvailableCardsTypes.weapon;
                    break;
                }
            case Card.CardsType.Armor:
                {
                    flags = Storage.AvailableCardsTypes.armor;
                    break;
                }
            case Card.CardsType.Other:
                {
                    flags = Storage.AvailableCardsTypes.other;
                    break;
                }
        }
        foreach (KeyValuePair<Storage.StorageNames, StorageUI> storage in storages)
        {
            if (storage.Value.IsActive && storage.Value.Storage.CardsTypes.HasFlag(flags))
            {
                res.Add((storage.Key, storage.Value.StorageVE));
            }
        }
        return res;
    }

    public void CardWasMoved(VisualElement cardVE, Card card, Storage.StorageNames prevStorage, 
        Storage.StorageNames newStorage)
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
    public Storage Storage { get; private set; }
    public bool IsActive { get; private set; }
    public VisualElement StorageVE { get; private set; }
    public VisualElement CardPanel { get; private set; }
    public List<(Card card, VisualElement cardVE)> Cards { get; private set; }

    public StorageUI(Storage storage, bool isActive, VisualElement storageVE, VisualElement cardPanel)
    {
        Storage = storage;
        IsActive = isActive;
        StorageVE = storageVE;
        CardPanel = cardPanel;
        Cards = new(Storage.Capacity);
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

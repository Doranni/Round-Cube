using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EquipmentUI : Singleton<EquipmentUI>
{
    [SerializeField] private Vector2 cardSize_small = new Vector2(80, 120), 
        cardSize_big = new Vector2(120, 160);
    [SerializeField] private float slotCardMargin = 5, inventoryCardMargin;
    private float slotWeaponPosLeft, slotArmorPosLeft, slotOtherPosLeft;

    [SerializeField] private Equipment plEquipment;

    private VisualElement plEquipmentScreen;
    private VisualElement inventoryButton, inventoryScreen;
    private VisualElement plSlotsCardsPanel, plInventoryCardsPanel;

    public Dictionary<Storage.StorageNames, StorageUI> storages = new();

    const string k_equipmentScreen = "PlayerEquipment";
    const string k_slotWeapon = "SlopWeapon";
    const string k_slotArmor = "SlotArmor";
    const string k_slotOther = "SlotOther";
    const string k_inventoryButton = "InventoryButton";
    const string k_inventoryScreen = "Inventory";
    const string k_inventoryContent = "InventoryContent";
    const string k_cardBackground = "CardBackground";
    const string k_cardName = "CardName";
    const string k_cardsPanel = "PlayerEquipCards";

    private VisualTreeAsset cardAsset;

    private bool isInventoryOpen = false;

    public event Action<bool> OnToggleOpenInvemtory;

    public override void Awake()
    {
        base.Awake();

        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        plEquipmentScreen = rootElement.Q(k_equipmentScreen);
        inventoryButton = rootElement.Q(k_inventoryButton);
        inventoryScreen = rootElement.Q(k_inventoryScreen);
        plSlotsCardsPanel = rootElement.Q(k_cardsPanel);
        plInventoryCardsPanel = rootElement.Q(k_inventoryContent);

        storages.Add(Storage.StorageNames.weaponSlot, new(plEquipment.Storages[Storage.StorageNames.weaponSlot],
            true, rootElement.Q(k_slotWeapon), plSlotsCardsPanel));
        storages.Add(Storage.StorageNames.armorSlot, new(plEquipment.Storages[Storage.StorageNames.armorSlot], 
            true, rootElement.Q(k_slotArmor), plSlotsCardsPanel));
        storages.Add(Storage.StorageNames.otherSlot, new(plEquipment.Storages[Storage.StorageNames.otherSlot], 
            true, rootElement.Q(k_slotOther), plSlotsCardsPanel));
        storages.Add(Storage.StorageNames.inventory, new(plEquipment.Storages[Storage.StorageNames.inventory], 
            false, plInventoryCardsPanel, plInventoryCardsPanel));

        slotWeaponPosLeft = storages[Storage.StorageNames.weaponSlot].StorageVE.style.left.value.value;
        slotArmorPosLeft = storages[Storage.StorageNames.armorSlot].StorageVE.style.left.value.value;
        slotOtherPosLeft = storages[Storage.StorageNames.otherSlot].StorageVE.style.left.value.value;

        cardAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;

        SetSize(storages[Storage.StorageNames.weaponSlot].StorageVE, cardSize_small);
        SetSize(storages[Storage.StorageNames.armorSlot].StorageVE, cardSize_small);
        SetSize(storages[Storage.StorageNames.otherSlot].StorageVE, cardSize_small);
        SetSize(inventoryButton, cardSize_small);
    }

    private void Start()
    {
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
        if (isInventoryOpen)
        {
            ToggleOpenInvemtory();
        }
    }

    private void SetSize(VisualElement vElement, Vector2 size)
    {
        vElement.style.width = size.x;
        vElement.style.height = size.y;
    }

    private void StyleCard(Storage.StorageNames storageName, VisualElement cardVE, Card card)
    {
        var cardBackgroung = cardVE.Q(k_cardBackground);
        var cardName = cardVE.Q<Label>(k_cardName);
        cardName.text = card.CardName;
        switch (storageName)
        {
            case Storage.StorageNames.weaponSlot:
                {
                    SetSize(cardBackgroung, cardSize_small);
                    cardVE.style.position = Position.Absolute;
                    cardVE.style.left = 0;
                    cardBackgroung.style.marginBottom = slotCardMargin;
                    cardBackgroung.style.marginTop = slotCardMargin;
                    cardBackgroung.style.marginLeft = slotCardMargin;
                    cardBackgroung.style.marginRight = slotCardMargin;
                    break;
                }
            case Storage.StorageNames.armorSlot:
                {
                    SetSize(cardBackgroung, cardSize_small);
                    cardVE.style.position = Position.Absolute;
                    cardVE.style.left = 90;
                    cardBackgroung.style.marginBottom = slotCardMargin;
                    cardBackgroung.style.marginTop = slotCardMargin;
                    cardBackgroung.style.marginLeft = slotCardMargin;
                    cardBackgroung.style.marginRight = slotCardMargin;
                    break;
                }
            case Storage.StorageNames.otherSlot:
                {
                    SetSize(cardBackgroung, cardSize_small);
                    cardVE.style.position = Position.Absolute;
                    cardVE.style.left = 180;
                    cardBackgroung.style.marginBottom = slotCardMargin;
                    cardBackgroung.style.marginTop = slotCardMargin;
                    cardBackgroung.style.marginLeft = slotCardMargin;
                    cardBackgroung.style.marginRight = slotCardMargin;
                    break;
                }
            case Storage.StorageNames.inventory:
                {
                    SetSize(cardBackgroung, cardSize_big);
                    cardVE.style.position = Position.Relative;
                    cardBackgroung.style.marginBottom = inventoryCardMargin;
                    cardBackgroung.style.marginTop = inventoryCardMargin;
                    cardBackgroung.style.marginLeft = inventoryCardMargin;
                    cardBackgroung.style.marginRight = inventoryCardMargin;
                    break;
                }
        }
    }

    private void DisplayInventoryButton()
    {
        if (plEquipment.Storages[Storage.StorageNames.inventory].Cards.Count == 0)
        {
            inventoryButton.style.display = DisplayStyle.None;
            if (isInventoryOpen)
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
        if (isInventoryOpen)
        {
            inventoryScreen.style.display = DisplayStyle.Flex;
            storages[Storage.StorageNames.inventory].SetIsActive(true);
        }
        else
        {
            inventoryScreen.style.display = DisplayStyle.None;
            storages[Storage.StorageNames.inventory].SetIsActive(false);
        }
    }

    private void ToggleOpenInvemtory()
    {
        isInventoryOpen = !isInventoryOpen;
        DisplayInventory();
        OnToggleOpenInvemtory?.Invoke(isInventoryOpen);
    }

    private void DisplayCards(Storage.StorageNames storageName)
    {
        storages[storageName].Update();
        for (int i = 0; i < storages[storageName].Cards.Count; i++)
        {
            var cardUI = cardAsset.CloneTree();
            storages[storageName].SetCardVE(i, cardUI);
            StyleCard(storageName, cardUI, storages[storageName].Cards[i].card);
            cardUI.AddManipulator(new DragAndDropManipulator(cardUI, cardSize_small, 
                storages[storageName].Cards[i].card, storageName));
        }
        if (storageName == Storage.StorageNames.otherSlot && storages[storageName].Storage.ActiveSlot != -1)
        {

            storages[storageName].Cards[storages[storageName].Storage.ActiveSlot].cardVE.BringToFront();
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

    public void CardWasMoved(Storage.StorageNames prevSlot, Storage.StorageNames newSlot)
    {
        if (prevSlot == newSlot)
        {
            DisplayCards(newSlot);
        }
        else
        {

        }
        Debug.Log($"Card was moved from {prevSlot} to {newSlot}");
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

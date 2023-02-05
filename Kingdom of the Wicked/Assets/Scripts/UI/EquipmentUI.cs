using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EquipmentUI : Singleton<EquipmentUI>
{
    [SerializeField] private Equipment plEquipment;

    private VisualElement plEquipmentScreen;
    private VisualElement inventoryButton, inventoryScreen;
    private VisualElement changeOtherSlotActiveCardButton, toggleArmorShieldButton;
    private Label otherSlotActiveCardLabel, toggleArmorShieldLabel;
    private VisualElement armorAndShieldSlots, plInventoryCardsPanel, dragCardPanel;

    public Dictionary<IStorage.StorageNames, StorageUI> storages = new();

    const string k_equipmentScreen = "PlayerEquipment";
    const string k_slotWeapon = "SlopWeapon";
    const string k_slotArmor = "SlotArmor";
    const string k_slotShield = "SlotShield";
    const string k_slotOther = "SlotOther";
    const string k_changeOtherSlotActiveCardButton = "ChangeOtherSlotActiveCardButton";
    const string k_otherSlotActiveCardLabel = "ActiveCard";
    const string k_toggleArmorShieldButton = "ToggleArmorShieldButton";
    const string k_toggleArmorShieldLabel = "ActiveSlot";
    const string k_inventoryButton = "InventoryButton";
    const string k_inventoryScreen = "Inventory";
    const string k_armorAndShieldSlots = "ArmorAndShieldSlots";
    const string k_inventoryContent = "InventoryContent";
    const string k_dragCardPanel = "DragCardPanel";

    private VisualTreeAsset cardAsset;

    private IStorage.StorageNames activeArmorShieldSlot;

    public event Action<bool> OnToggleOpenInvemtory;

    public override void Awake()
    {
        base.Awake();

        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        plEquipmentScreen = rootElement.Q(k_equipmentScreen);
        inventoryButton = rootElement.Q(k_inventoryButton);
        inventoryScreen = rootElement.Q(k_inventoryScreen);
        armorAndShieldSlots = rootElement.Q(k_armorAndShieldSlots);
        plInventoryCardsPanel = rootElement.Q(k_inventoryContent);
        dragCardPanel = rootElement.Q(k_dragCardPanel);
        changeOtherSlotActiveCardButton = rootElement.Q(k_changeOtherSlotActiveCardButton);
        otherSlotActiveCardLabel = rootElement.Q<Label>(k_otherSlotActiveCardLabel);
        toggleArmorShieldButton = rootElement.Q(k_toggleArmorShieldButton);
        toggleArmorShieldLabel = rootElement.Q<Label>(k_toggleArmorShieldLabel);

        storages.Add(IStorage.StorageNames.weaponSlot, new(plEquipment.Storages[IStorage.StorageNames.weaponSlot],
            true, rootElement.Q(k_slotWeapon), rootElement.Q(k_slotWeapon)));
        storages.Add(IStorage.StorageNames.armorSlot, new(plEquipment.Storages[IStorage.StorageNames.armorSlot], 
            true, rootElement.Q(k_slotArmor), rootElement.Q(k_slotArmor)));
        storages.Add(IStorage.StorageNames.shieldSlot, new(plEquipment.Storages[IStorage.StorageNames.shieldSlot],
            true, rootElement.Q(k_slotShield), rootElement.Q(k_slotShield)));
        storages.Add(IStorage.StorageNames.otherSlot, new(plEquipment.Storages[IStorage.StorageNames.otherSlot], 
            true, rootElement.Q(k_slotOther), rootElement.Q(k_slotOther)));
        storages.Add(IStorage.StorageNames.inventory, new(plEquipment.Storages[IStorage.StorageNames.inventory], 
            false, plInventoryCardsPanel, plInventoryCardsPanel));

        cardAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;

        UIManager.Instance.SetSize(storages[IStorage.StorageNames.weaponSlot].StorageVE,
            IStorage.StorageNames.weaponSlot);
        UIManager.Instance.SetSize(armorAndShieldSlots, IStorage.StorageNames.armorSlot);
        UIManager.Instance.SetSize(storages[IStorage.StorageNames.otherSlot].StorageVE,
            IStorage.StorageNames.otherSlot);
        UIManager.Instance.SetSize(inventoryButton, IStorage.StorageNames.weaponSlot);

        activeArmorShieldSlot = IStorage.StorageNames.armorSlot;
    }

    private void Start()
    {
        DragAndDropController.Instance.SetCardToDrag(cardAsset.CloneTree(), dragCardPanel);

        plEquipment.OnStorageChanged += DisplayCards;

        inventoryButton.RegisterCallback<ClickEvent>(_ => ToggleOpenInvemtory()) ;
        InputManager.Instance.OnUIEscape_performed += _ => GameUIEscape_performed();
        changeOtherSlotActiveCardButton.RegisterCallback<ClickEvent>(_ => ChangeOtherSlotActiveCard());
        toggleArmorShieldButton.RegisterCallback<ClickEvent>(_ => ToggleArmorShieldSlot());

        DisplayCards(IStorage.StorageNames.weaponSlot);
        DisplayCards(IStorage.StorageNames.armorSlot);
        DisplayCards(IStorage.StorageNames.shieldSlot);
        DisplayCards(IStorage.StorageNames.otherSlot);
        DisplayCards(IStorage.StorageNames.inventory);
        DispleyActiveArmorShieldSlot();
        DisplayInventory();
    }

    private void ChangeOtherSlotActiveCard()
    {
        ((Slot)plEquipment.Storages[IStorage.StorageNames.otherSlot]).ChangeActiveCardIndex();
        DisplayOtherSlotActiveCard();
    }

    private void DisplayOtherSlotActiveCard()
    {
        var otherSlot = (Slot)plEquipment.Storages[IStorage.StorageNames.otherSlot];
        Debug.Log($"Other slot cards.Count - " + otherSlot.Cards.Count);
        for(int i = 0; i < storages[otherSlot.StorageName].Cards.Count; i++)
        {
            if (i == otherSlot.ActiveCardIndex)
            {
                storages[otherSlot.StorageName].Cards[i].cardVE.style.display = DisplayStyle.Flex;
            }
            else
            {
                storages[otherSlot.StorageName].Cards[i].cardVE.style.display = DisplayStyle.None;
            }
        }
        otherSlotActiveCardLabel.text = (otherSlot.ActiveCardIndex + 1).ToString();
    }

    private void ToggleArmorShieldSlot()
    {
        if (activeArmorShieldSlot == IStorage.StorageNames.armorSlot)
        {
            activeArmorShieldSlot = IStorage.StorageNames.shieldSlot;
        }
        else
        {
            activeArmorShieldSlot = IStorage.StorageNames.armorSlot;
        }
        DispleyActiveArmorShieldSlot();
    }

    private void DispleyActiveArmorShieldSlot()
    {
        if (activeArmorShieldSlot == IStorage.StorageNames.armorSlot)
        {
            if(storages[IStorage.StorageNames.armorSlot].Cards.Count > 0)
            {
                storages[IStorage.StorageNames.armorSlot].Cards[0].cardVE.style.display = DisplayStyle.Flex;
            }
            if(storages[IStorage.StorageNames.shieldSlot].Cards.Count > 0)
            {
                storages[IStorage.StorageNames.shieldSlot].Cards[0].cardVE.style.display = DisplayStyle.None;
            }
            toggleArmorShieldLabel.text = "A";
        }
        else
        {
            if (storages[IStorage.StorageNames.armorSlot].Cards.Count > 0)
            {
                storages[IStorage.StorageNames.armorSlot].Cards[0].cardVE.style.display = DisplayStyle.None;
            }
            if (storages[IStorage.StorageNames.shieldSlot].Cards.Count > 0)
            {
                storages[IStorage.StorageNames.shieldSlot].Cards[0].cardVE.style.display = DisplayStyle.Flex;
            }
            toggleArmorShieldLabel.text = "S";
        }
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
        switch (storageName)
        {
            case IStorage.StorageNames.armorSlot:
            case IStorage.StorageNames.shieldSlot:
                {
                    DispleyActiveArmorShieldSlot();
                    break;
                }
            case IStorage.StorageNames.otherSlot:
                {
                    DisplayOtherSlotActiveCard();
                    break;
                }
            case IStorage.StorageNames.inventory:
                {
                    DisplayInventoryButton();
                    break;
                }
        }
    }

    public List<(IStorage.StorageNames storageName, VisualElement storageUI)> GetAvailableStorages(
        Card.CardsType type)
    {
        List<(IStorage.StorageNames, VisualElement)> res = new();
        foreach (KeyValuePair<IStorage.StorageNames, StorageUI> storage in storages)
        {
            if (storage.Value.IsActive)
            {
                res.Add((storage.Key, storage.Value.StorageVE));
            }
        }
        return res;
    }

    public void CardWasMoved(Card card, IStorage.StorageNames prevStorage, 
        List<IStorage.StorageNames> newStorages)
    {
        if (newStorages.Count == 0 && 
            (prevStorage != IStorage.StorageNames.inventory && prevStorage != IStorage.StorageNames.storage))
        {
            plEquipment.MoveCard(card, prevStorage, IStorage.StorageNames.inventory);
            Debug.Log($"Card was moved from {prevStorage} to inventory");
            return;
        }
        foreach(IStorage.StorageNames storage in newStorages)
        {
            if (prevStorage != storage
            && ((storages[storage].Storage.CardsTypes.HasFlag(Card.CardsType.Weapon)
                && card.CardType.HasFlag(Card.CardsType.Weapon))
            || (storages[storage].Storage.CardsTypes.HasFlag(Card.CardsType.Armor)
                && card.CardType.HasFlag(Card.CardsType.Armor))
            || (storages[storage].Storage.CardsTypes.HasFlag(Card.CardsType.Shield)
                && card.CardType.HasFlag(Card.CardsType.Shield))
            || (storages[storage].Storage.CardsTypes.HasFlag(Card.CardsType.Other)
                && card.CardType.HasFlag(Card.CardsType.Other))))
            {
                plEquipment.MoveCard(card, prevStorage, storage);
                Debug.Log($"Card was moved from {prevStorage} to {storage}");
                return;
            }
        }
    }
}

public class StorageUI
{
    public IStorage Storage { get; private set; }
    public bool IsActive { get; private set; }
    public VisualElement StorageVE { get; private set; }
    public List<(Card card, VisualElement cardVE)> Cards { get; private set; }

    public StorageUI(IStorage storage, bool isActive, VisualElement storageVE, VisualElement cardPanel)
    {
        Storage = storage;
        IsActive = isActive;
        StorageVE = storageVE;
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
            StorageVE.Remove(Cards[i].cardVE);
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
            StorageVE.Add(cardVE);
        }
    }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
}

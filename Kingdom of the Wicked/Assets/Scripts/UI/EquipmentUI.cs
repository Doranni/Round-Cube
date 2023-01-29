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
    [SerializeField] private float inventoryCardMargin;
    [SerializeField] private Vector2 dragRangeMin = Vector2.zero,
        dragRangeMax = new Vector2(1920, 1080);
    public Vector2 DragRangeMin => dragRangeMin;
    public Vector2 DragRangeMax => dragRangeMax;

    [SerializeField] private Equipment plEquipment;
    [SerializeField] private Inventory plInventory;

    private VisualElement plEquipmentScreen;
    public Dictionary<Storage.StorageNames, (Storage.AvailableCardsTypes type, bool isActive, VisualElement slot,
        VisualElement card)> slots = new();
    private VisualElement inventoryButton, inventoryScreen;
    private VisualElement plCardsPanel;

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

    VisualTreeAsset CardAsset;

    private bool isInventoryOpen = false;

    public event Action<bool> OnToggleOpenInvemtory;

    public override void Awake()
    {
        base.Awake();

        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        slots.Add(Storage.StorageNames.weaponSlot, (Storage.AvailableCardsTypes.weapon, true, rootElement.Q(k_slotWeapon), null));
        slots.Add(Storage.StorageNames.armorSlot, (Storage.AvailableCardsTypes.armor, true, rootElement.Q(k_slotArmor), null));
        slots.Add(Storage.StorageNames.otherSlot, (Storage.AvailableCardsTypes.other, true, rootElement.Q(k_slotOther), null));
        slots.Add(Storage.StorageNames.inventory, (Storage.AvailableCardsTypes.weapon | Storage.AvailableCardsTypes.armor 
            | Storage.AvailableCardsTypes.other, 
            false, rootElement.Q(k_inventoryContent), null));

        plEquipmentScreen = rootElement.Q(k_equipmentScreen);
        inventoryButton = rootElement.Q(k_inventoryButton);
        inventoryScreen = rootElement.Q(k_inventoryScreen);
        plCardsPanel = rootElement.Q(k_cardsPanel);

        CardAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;

        SetSize(slots[Storage.StorageNames.weaponSlot].slot, cardSize_small);
        SetSize(slots[Storage.StorageNames.armorSlot].slot, cardSize_small);
        SetSize(slots[Storage.StorageNames.otherSlot].slot, cardSize_small);
        SetSize(inventoryButton, cardSize_small);
    }

    private void Start()
    {
        plEquipment.OnEquippedWeaponCardChanged += delegate 
        { DisplayCardOnSlot(Storage.StorageNames.weaponSlot); };
        plEquipment.OnEquippedArmorCardChanged += delegate 
        { DisplayCardOnSlot(Storage.StorageNames.armorSlot); };
        plEquipment.OnEquippedOtherCardChanged += delegate 
        { DisplayCardOnSlot(Storage.StorageNames.otherSlot); };

        plInventory.OnInventoryChanged += DisplayInventoryButton;
        plInventory.OnInventoryChanged += UpdateInventory;
        inventoryButton.RegisterCallback<ClickEvent>(_ => ToggleOpenInvemtory()) ;
        InputManager.Instance.OnUIEscape_performed += _ => GameUIEscape_performed();

        DisplayCardOnSlot(Storage.StorageNames.weaponSlot);
        DisplayCardOnSlot(Storage.StorageNames.armorSlot);
        DisplayCardOnSlot(Storage.StorageNames.otherSlot);
        DisplayInventoryButton();
        UpdateInventory();
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

    private void StyleCard(VisualElement cardVE, Card card, Vector2 size, float margin)
    {
        var cardBackgroung = cardVE.Q(k_cardBackground);
        var cardName = cardVE.Q<Label>(k_cardName);
        cardBackgroung.style.width = size.x;
        cardBackgroung.style.height = size.y;
        cardName.text = card.CardName;
        cardBackgroung.style.marginBottom = margin;
        cardBackgroung.style.marginTop = margin;
        cardBackgroung.style.marginLeft = margin;
        cardBackgroung.style.marginRight = margin;
    }

    private void DisplayCardOnSlot(Storage.StorageNames slotName)
    {
        switch (slotName)
        {
            case Storage.StorageNames.weaponSlot:
                {
                    DisplayCardOnSlot(Storage.StorageNames.weaponSlot, plEquipment.WeaponCard, 0);
                    break;
                }
            case Storage.StorageNames.armorSlot:
                {
                    DisplayCardOnSlot(Storage.StorageNames.armorSlot, plEquipment.ArmorCard, 90);
                    break;
                }
            case Storage.StorageNames.otherSlot:
                {
                    DisplayCardOnSlot(Storage.StorageNames.otherSlot, 
                        plEquipment.OtherCards[plEquipment.ActiveOtherSlot], 180);
                    break;
                }
            case Storage.StorageNames.inventory:
                {
                    UpdateInventory();
                    break;
                }
        }
    }

    private void DisplayCardOnSlot(Storage.StorageNames slotName, Card newCard, float posLeft)
    {
        if (slots[slotName].card != null)
        {
            plCardsPanel.Remove(slots[slotName].card);
        }
        if (newCard != null)
        {
            VisualElement cardVE = CardAsset.CloneTree();
            plCardsPanel.Add(cardVE);
            cardVE.style.position = Position.Absolute;
            cardVE.style.left = posLeft;
            StyleCard(cardVE, newCard, cardSize_small, 5);
            slots[slotName]= (slots[slotName].type, slots[slotName].isActive, 
                slots[slotName].slot, cardVE);
            cardVE.AddManipulator(new DragAndDropManipulator(cardVE, cardSize_small, 
                newCard.CardType, slotName));
        }
    }

    private void DisplayInventoryButton()
    {
        if (plInventory.Cards.Count == 0)
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
            SetSlotIsActive(Storage.StorageNames.inventory, true);
        }
        else
        {
            inventoryScreen.style.display = DisplayStyle.None;
            SetSlotIsActive(Storage.StorageNames.inventory, false);
        }
    }

    private void ToggleOpenInvemtory()
    {
        isInventoryOpen = !isInventoryOpen;
        DisplayInventory();
        OnToggleOpenInvemtory?.Invoke(isInventoryOpen);
    }

    private void UpdateInventory()
    {
        slots[Storage.StorageNames.inventory].slot.Clear();
        foreach (Card card in plInventory.Cards)
        {
            var cartToDisplay = CardAsset.CloneTree();
            StyleCard(cartToDisplay, card, cardSize_big, inventoryCardMargin);
            slots[Storage.StorageNames.inventory].slot.Add(cartToDisplay);
        }
    }

    public void SetSlotIsActive(Storage.StorageNames name, bool isActive)
    {
        if (slots.ContainsKey(name))
        {
            slots[name] = (slots[name].type, isActive, slots[name].slot, slots[name].card);
        }
    }

    public List<(Storage.StorageNames slotName, VisualElement slot)> GetAvailableSlots(Card.CardsType type)
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
        foreach (KeyValuePair<Storage.StorageNames,
                        (Storage.AvailableCardsTypes type, bool isActive, VisualElement slot, VisualElement card)>
                        slot in slots)
        {
            if (slot.Value.isActive && slot.Value.type.HasFlag(flags))
            {
                res.Add((slot.Key, slot.Value.slot));
            }
        }
        return res;
    }

    public void CardWasMoved(Storage.StorageNames prevSlot, Storage.StorageNames newSlot)
    {
        if (prevSlot == newSlot)
        {
            DisplayCardOnSlot(newSlot);
        }
        else
        {

        }
        Debug.Log($"Card was moved from {prevSlot} to {newSlot}");
    }
}

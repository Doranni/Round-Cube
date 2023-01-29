using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EquipmentUI : Singleton<EquipmentUI>
{
    [Flags]
    public enum SlotType
    {
        weapon = 1,
        armor = 2,
        other = 4
    }
    public enum SlotNames
    {
        weaponSlot,
        armorSlot,
        otherSlot,
        inventory,
        storage
    }

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
    public Dictionary<SlotNames, (SlotType type, bool isActive, VisualElement slot,
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

        slots.Add(SlotNames.weaponSlot, (SlotType.weapon, true, rootElement.Q(k_slotWeapon), null));
        slots.Add(SlotNames.armorSlot, (SlotType.armor, true, rootElement.Q(k_slotArmor), null));
        slots.Add(SlotNames.otherSlot, (SlotType.other, true, rootElement.Q(k_slotOther), null));
        slots.Add(SlotNames.inventory, (SlotType.weapon | SlotType.armor | SlotType.other, 
            false, rootElement.Q(k_inventoryContent), null));

        plEquipmentScreen = rootElement.Q(k_equipmentScreen);
        inventoryButton = rootElement.Q(k_inventoryButton);
        inventoryScreen = rootElement.Q(k_inventoryScreen);
        plCardsPanel = rootElement.Q(k_cardsPanel);

        CardAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;

        SetSize(slots[SlotNames.weaponSlot].slot, cardSize_small);
        SetSize(slots[SlotNames.armorSlot].slot, cardSize_small);
        SetSize(slots[SlotNames.otherSlot].slot, cardSize_small);
        SetSize(inventoryButton, cardSize_small);
    }

    private void Start()
    {
        plEquipment.OnEquippedWeaponCardChanged += delegate 
        { DisplayCardOnSlot(SlotNames.weaponSlot, plEquipment.WeaponCard, 0); };
        plEquipment.OnEquippedArmorCardChanged += delegate 
        { DisplayCardOnSlot(SlotNames.armorSlot, plEquipment.ArmorCard, 90); };
        plEquipment.OnEquippedOtherCardChanged += delegate 
        { DisplayCardOnSlot(SlotNames.otherSlot, 
            plEquipment.OtherCards[plEquipment.ActiveOtherSlot], 180); };

        plInventory.OnInventoryChanged += DisplayInventoryButton;
        plInventory.OnInventoryChanged += UpdateInventory;
        inventoryButton.RegisterCallback<ClickEvent>(_ => ToggleOpenInvemtory()) ;
        InputManager.Instance.OnUIEscape_performed += _ => GameUIEscape_performed();

        DisplayCardOnSlot(SlotNames.weaponSlot, plEquipment.WeaponCard, 0);
        DisplayCardOnSlot(SlotNames.armorSlot, plEquipment.ArmorCard, 90);
        DisplayCardOnSlot(SlotNames.otherSlot, plEquipment.OtherCards[plEquipment.ActiveOtherSlot], 180);
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

    private void DisplayCardOnSlot(SlotNames slotName, Card newCard, float posLeft)
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
            SetSlotIsActive(SlotNames.inventory, true);
        }
        else
        {
            inventoryScreen.style.display = DisplayStyle.None;
            SetSlotIsActive(SlotNames.inventory, false);
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
        slots[SlotNames.inventory].slot.Clear();
        foreach (Card card in plInventory.Cards)
        {
            var cartToDisplay = CardAsset.CloneTree();
            StyleCard(cartToDisplay, card, cardSize_big, inventoryCardMargin);
            slots[SlotNames.inventory].slot.Add(cartToDisplay);
        }
    }

    public void SetSlotIsActive(SlotNames name, bool isActive)
    {
        if (slots.ContainsKey(name))
        {
            slots[name] = (slots[name].type, isActive, slots[name].slot, slots[name].card);
        }
    }

    public List<VisualElement> GetAvailableSlots(Card.CardsType type)
    {
        List<VisualElement> res = new();
        switch (type)
        {
            case Card.CardsType.Weapon:
                {
                    foreach ((SlotType type, bool isActive, VisualElement slot, VisualElement card) 
                        slot in slots.Values)
                    {
                        if (slot.isActive && slot.type.HasFlag(SlotType.weapon))
                        {
                            res.Add(slot.slot);
                        }
                    }
                    break;
                }
            case Card.CardsType.Armor:
                {
                    foreach ((SlotType type, bool isActive, VisualElement slot, VisualElement card) 
                        slot in slots.Values)
                    {
                        if (slot.isActive && slot.type.HasFlag(SlotType.armor))
                        {
                            res.Add(slot.slot);
                        }
                    }
                    break;
                }
            case Card.CardsType.Other:
                {
                    foreach ((SlotType type, bool isActive, VisualElement slot, VisualElement card) 
                        slot in slots.Values)
                    {
                        if (slot.isActive && slot.type.HasFlag(SlotType.other))
                        {
                            res.Add(slot.slot);
                        }
                    }
                    break;
                }
        }
        return res;
    }
}

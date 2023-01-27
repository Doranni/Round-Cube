using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private Vector2 cardSize_small, cardSize_big;
    [SerializeField] private float inventoryCardMargin;

    [SerializeField] private Equipment plEquipment;
    [SerializeField] private Inventory plInventory;

    private VisualElement equipmentScreen;
    private VisualElement slotWeapon, slotArmor, slotOther;
    private VisualElement inventoryButton, inventoryScreen, inventoryContent;



    const string k_equipmentScreen = "Equipment";
    const string k_slotWeapon = "SlopWeapon";
    const string k_slotArmor = "SlotArmor";
    const string k_slotOther = "SlotOther";
    const string k_inventoryButton = "InventoryButton";
    const string k_inventoryScreen = "Inventory";
    const string k_inventoryContent = "InventoryContent";
    const string k_cardBackground = "CardBackground";
    const string k_cardName = "CardName";

    VisualTreeAsset CardAsset;

    private bool isInventoryOpen = false;

    public event Action<bool> OnToggleOpenInvemtory;

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        equipmentScreen = rootElement.Q(k_equipmentScreen);
        slotWeapon = rootElement.Q(k_slotWeapon);
        slotArmor = rootElement.Q(k_slotArmor);
        slotOther = rootElement.Q(k_slotOther);
        inventoryButton = rootElement.Q(k_inventoryButton);
        inventoryScreen = rootElement.Q(k_inventoryScreen);
        inventoryContent = rootElement.Q(k_inventoryContent);

        CardAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;
    }

    private void Start()
    {
        SetSize(slotWeapon, cardSize_small);
        SetSize(slotArmor, cardSize_small);
        SetSize(slotOther, cardSize_small);
        SetSize(inventoryButton, cardSize_small);

        plEquipment.OnEquippedWeaponCardChanged += delegate { DisplayCardOnSlot(slotWeapon, plEquipment.WeaponCard); };
        plEquipment.OnEquippedArmorCardChanged += delegate { DisplayCardOnSlot(slotArmor, plEquipment.ArmorCard); };
        plEquipment.OnEquippedOtherCardChanged += delegate { DisplayCardOnSlot(slotOther, 
            plEquipment.OtherCards[plEquipment.ActiveOtherSlot]); };

        plInventory.OnInventoryChanged += DisplayInventoryButton;
        plInventory.OnInventoryChanged += UpdateInventory;
        inventoryButton.RegisterCallback<ClickEvent>(_ => ToggleOpenInvemtory()) ;
        InputManager.Instance.OnUIEscape_performed += _ => GameUIEscape_performed();

        DisplayCardOnSlot(slotWeapon, plEquipment.WeaponCard);
        DisplayCardOnSlot(slotArmor, plEquipment.ArmorCard);
        DisplayCardOnSlot(slotOther, plEquipment.OtherCards[plEquipment.ActiveOtherSlot]);
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

    private void DisplayCardOnSlot(VisualElement slot, Card card)
    {
        slot.Clear();
        if (card != null)
        {
            VisualElement cardVE = CardAsset.CloneTree();
            slot.Add(cardVE);
            StyleCard(cardVE, card, cardSize_small, 0);
            cardVE.RegisterCallback<MouseDownEvent>(_ =>
            { 
                DragCard.Instance.StartDragging(cardVE.Q(k_cardBackground)); 
            });
            cardVE.RegisterCallback<MouseUpEvent>(_ => 
            { 
                DragCard.Instance.StopDragging(cardVE.Q(k_cardBackground)); 
            });
            //cardVE.RegisterCallback<MouseOutEvent>(_ =>
            //{
            //    DragCard.Instance.StopDragging(cardVE.Q(k_cardBackground));
            //});
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
        }
        else
        {
            inventoryScreen.style.display = DisplayStyle.None;
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
        inventoryContent.Clear();
        foreach (Card card in plInventory.Cards)
        {
            var cartToDisplay = CardAsset.CloneTree();
            StyleCard(cartToDisplay, card, cardSize_big, inventoryCardMargin);
            inventoryContent.Add(cartToDisplay);
        }
    }
}

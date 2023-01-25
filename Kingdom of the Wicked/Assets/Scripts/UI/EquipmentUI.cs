using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EquipmentUI : MonoBehaviour
{
    [SerializeField] private Equipment equipment;

    private VisualElement equipmentScreen;
    private VisualElement slotWeapon, slotArmor, slotOther;
    private bool isWeaponCardClicked, isArmorCardClicked, isOtherCardClicked;

    const string k_equipmentScreen = "Equipment";
    const string k_slotWeapon = "SlopWeapon";
    const string k_slotArmor = "SlotArmor";
    const string k_slotOther = "SlotOther";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        equipmentScreen = rootElement.Q(k_equipmentScreen);
        slotWeapon = rootElement.Q(k_slotWeapon);
        slotArmor = rootElement.Q(k_slotArmor);
        slotOther = rootElement.Q(k_slotOther);
    }

    private void Start()
    {
        equipment.OnEquippedWeaponCardChanged += delegate { DisplayCard(slotWeapon, equipment.WeaponCard); };
        equipment.OnEquippedArmorCardChanged += delegate { DisplayCard(slotArmor, equipment.ArmorCard); };
        equipment.OnEquippedOtherCardChanged += delegate { DisplayCard(slotOther, 
            equipment.OtherCards[equipment.ActiveOtherSlot]); };

        slotWeapon.RegisterCallback<MouseDownEvent, VisualElement>(DragCard, slotWeapon);
        slotArmor.RegisterCallback<MouseDownEvent, VisualElement>(DragCard, slotArmor);
        slotOther.RegisterCallback<MouseDownEvent, VisualElement>(DragCard, slotOther);

        DisplayCard(slotWeapon, equipment.WeaponCard);
        DisplayCard(slotArmor, equipment.ArmorCard);
        DisplayCard(slotOther, equipment.OtherCards[equipment.ActiveOtherSlot]);
    }

    private void DragCard(MouseDownEvent evt, VisualElement slot)
    {
        Debug.Log($"{slot.name} MouseDownEvent");

    }

    private void DisplayCard(VisualElement slot, Card card)
    {
        slot.Clear();
        if (card != null)
        {
            VisualTreeAsset uiAsset = EditorGUIUtility.Load("Assets/UI/CardUI.uxml") as VisualTreeAsset;
            VisualElement ui = uiAsset.CloneTree();
            slot.Add(ui);
        }
    }
}

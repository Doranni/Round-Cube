using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class DragCardManager : Singleton<DragCardManager>
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

    [SerializeField] private Vector2 dragRangeMin = Vector2.zero, 
        dragRangeMax = new Vector2(1920, 1080);
    public Vector2 DragRangeMin => dragRangeMin;
    public Vector2 DragRangeMax => dragRangeMax;

    private List<VisualElement> targets = new();
    [SerializeField]
    public Dictionary<SlotNames, (SlotType type, bool isActive, VisualElement slot)> slots = new();

    public void AddTarget(VisualElement cardUI, Vector2 size, Card card)
    {
        cardUI.AddManipulator(new DragCardManipulator(cardUI, size, card.CardType));
    }

    public void AddSlot(VisualElement slot, SlotType type, SlotNames name, bool isActive)
    {
        slots.Add(name, (type, isActive, slot));
    }

    public void SetSlotIsActive(SlotNames name, bool isActive)
    {
        if (slots.ContainsKey(name))
        {
            slots[name] = (slots[name].type, isActive, slots[name].slot);
        }
    }

    public List<VisualElement> GetAvailableSlots(Card.CardsType type)
    {
        List<VisualElement> res = new();
        switch (type)
        {
            case Card.CardsType.Weapon:
                {
                    foreach((SlotType type, bool isActive, VisualElement slot) slot in slots.Values)
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
                    foreach ((SlotType type, bool isActive, VisualElement slot) slot in slots.Values)
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
                    foreach ((SlotType type, bool isActive, VisualElement slot) slot in slots.Values)
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

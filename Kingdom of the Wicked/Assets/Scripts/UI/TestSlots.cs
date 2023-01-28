using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TestSlots : MonoBehaviour
{
    private VisualElement inventory;
    private List<VisualElement> slots = new();

    const string k_slots = "Slots";
    const string k_slot = "Slot";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        inventory = rootElement.Q(k_slots);
        var slots = inventory.Children().ToList();
        foreach (VisualElement slot in slots)
        {
            if (slot.name == k_slot)
            {
                this.slots.Add(slot);
                
            }
        }
        DragAndDropManipulator manipulator = new(rootElement.Q<VisualElement>("Object"));
    }
}

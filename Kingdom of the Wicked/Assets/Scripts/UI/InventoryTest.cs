using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryTest : Singleton<InventoryTest>
{
    private VisualElement inventory, equipment;
    private List<VisualElement> cardSlots = new();

    public List<VisualElement> CardSlots => cardSlots;


    const string k_name_inventory = "InventoryContent";
    const string k_name_equipment = "Equipment";
    const string k_class_cardSlots = "cardSlot";

    public override void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        VisualElement inventory = rootElement.Q<VisualElement>(k_name_inventory);
        VisualElement equipment = rootElement.Q<VisualElement>(k_name_equipment);

        var invSlots = inventory.Children().ToList();
        foreach(VisualElement slot in invSlots)
        {
            if (slot.ClassListContains(k_class_cardSlots))
            {
                cardSlots.Add(slot);
            }
        }
        var equipSlots = equipment.Children().ToList();
        foreach (VisualElement slot in equipSlots)
        {
            if (slot.ClassListContains(k_class_cardSlots))
            {
                cardSlots.Add(slot);
            }
        }
        Debug.Log("InventoryTest, CardSlots count - " + CardSlots.Count);
        foreach(VisualElement slot in cardSlots)
        {
            Debug.Log(slot.name);
        }
    }
}

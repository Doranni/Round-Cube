using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Equipment))]
[RequireComponent(typeof(Inventory))]
public class PlayerController : MonoBehaviour
{
    private Equipment equipment;
    private Inventory inventory;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
    }

    void Start()
    {
        //test
        equipment.EquipWeaponCard(ItemsDatabase.Instance.GetCard(101));
        equipment.EquipArmorCard(ItemsDatabase.Instance.GetCard(201));
        //equipment.EquipOtherCard(cardOther);

        for(int i = 0; i< 2; i++)
        {
            inventory.AddCard(ItemsDatabase.Instance.GetCard(101));
            inventory.AddCard(ItemsDatabase.Instance.GetCard(201));
        }
    }
}

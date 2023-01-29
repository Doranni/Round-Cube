using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Equipment))]
public class PlayerController : MonoBehaviour
{
    private Equipment equipment;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
    }

    void Start()
    {
        //test
        equipment.AddCard(ItemsDatabase.Instance.GetCard(101), Storage.StorageNames.weaponSlot);
        equipment.AddCard(ItemsDatabase.Instance.GetCard(201), Storage.StorageNames.armorSlot);
        //equipment.EquipOtherCard(cardOther);

        for(int i = 0; i< 2; i++)
        {
            equipment.AddCard(ItemsDatabase.Instance.GetCard(101), Storage.StorageNames.inventory);
            equipment.AddCard(ItemsDatabase.Instance.GetCard(201), Storage.StorageNames.inventory);
        }
    }
}

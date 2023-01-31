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
        equipment.AddCard(ItemsDatabase.Instance.GetCard(101), IStorage.StorageNames.weaponSlot);
        equipment.AddCard(ItemsDatabase.Instance.GetCard(201), IStorage.StorageNames.armorSlot);

        equipment.AddCard(ItemsDatabase.Instance.GetCard(102), IStorage.StorageNames.inventory);
        equipment.AddCard(ItemsDatabase.Instance.GetCard(202), IStorage.StorageNames.inventory);
    }
}

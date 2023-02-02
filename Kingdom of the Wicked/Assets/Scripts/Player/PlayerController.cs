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
        equipment.AddCard(GameDatabase.Instance.GetCard(101), IStorage.StorageNames.weaponSlot);
        equipment.AddCard(GameDatabase.Instance.GetCard(201), IStorage.StorageNames.armorSlot);

        //equipment.AddCard(GameDatabase.Instance.GetCard(102), IStorage.StorageNames.inventory);
        //equipment.AddCard(GameDatabase.Instance.GetCard(202), IStorage.StorageNames.inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(301), IStorage.StorageNames.inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(302), IStorage.StorageNames.inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(303), IStorage.StorageNames.inventory);
        //equipment.AddCard(GameDatabase.Instance.GetCard(304), IStorage.StorageNames.inventory);
    }
}

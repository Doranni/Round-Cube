using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterEquipment))]
public class PlayerController : MonoBehaviour
{
    private CharacterEquipment equipment;

    private void Awake()
    {
        equipment = GetComponent<CharacterEquipment>();
    }

    void Start()
    {
        //test
        equipment.AddCard(GameDatabase.Instance.GetCard(101), IStorage.StorageNames.WeaponSlot);
        equipment.AddCard(GameDatabase.Instance.GetCard(201), IStorage.StorageNames.ArmorSlot);

        equipment.AddCard(GameDatabase.Instance.GetCard(102), IStorage.StorageNames.WeaponSlot);
        equipment.AddCard(GameDatabase.Instance.GetCard(301), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(302), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(401), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(402), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(403), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(404), IStorage.StorageNames.Inventory);
    }
}

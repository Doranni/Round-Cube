using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterEquipment))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CharacterEquipment equipment;

    void Start()
    {
        //test
        equipment.AddCard(GameDatabase.Instance.GetCard(101), IStorage.StorageNames.WeaponSlot);
        equipment.AddCard(GameDatabase.Instance.GetCard(201), IStorage.StorageNames.ArmorSlot);

        equipment.AddCard(GameDatabase.Instance.GetCard(102), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(202), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(301), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(302), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(401), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(402), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(501), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(502), IStorage.StorageNames.WeaponSlot);
        equipment.AddCard(GameDatabase.Instance.GetCard(601), IStorage.StorageNames.Inventory);
        equipment.AddCard(GameDatabase.Instance.GetCard(602), IStorage.StorageNames.Inventory);

        ((IUsable)equipment.Storages[IStorage.StorageNames.WeaponSlot].Cards[0]).Use(GetComponent<Character>());
    }
}

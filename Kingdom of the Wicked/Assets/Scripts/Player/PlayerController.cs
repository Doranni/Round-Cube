using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Character player;

    void Start()
    {

        //test
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(101), IStorage.StorageNames.WeaponSlot);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(201), IStorage.StorageNames.ArmorSlot);

        player.Equipment.AddCard(GameDatabase.Instance.GetCard(102), IStorage.StorageNames.Inventory);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(202), IStorage.StorageNames.Inventory);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(301), IStorage.StorageNames.Inventory);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(302), IStorage.StorageNames.Inventory);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(401), IStorage.StorageNames.Inventory);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(402), IStorage.StorageNames.Inventory);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(501), IStorage.StorageNames.Inventory);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(502), IStorage.StorageNames.WeaponSlot);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(601), IStorage.StorageNames.Inventory);
        player.Equipment.AddCard(GameDatabase.Instance.GetCard(602), IStorage.StorageNames.Inventory);

        ((IUsable)player.Equipment.Storages[IStorage.StorageNames.WeaponSlot].Cards[0]).Use(GetComponent<Character>());
    }
}

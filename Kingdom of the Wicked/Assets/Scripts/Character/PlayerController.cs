using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Character
{
    protected override void Start()
    {
        base.Start();

        //test
        Equipment.AddCard(GameDatabase.Instance.GetCard(101), IStorage.StorageNames.WeaponSlot);
        Equipment.AddCard(GameDatabase.Instance.GetCard(201), IStorage.StorageNames.ArmorSlot);

        Equipment.AddCard(GameDatabase.Instance.GetCard(102), IStorage.StorageNames.Inventory);
        Equipment.AddCard(GameDatabase.Instance.GetCard(202), IStorage.StorageNames.Inventory);
        Equipment.AddCard(GameDatabase.Instance.GetCard(301), IStorage.StorageNames.Inventory);
        Equipment.AddCard(GameDatabase.Instance.GetCard(302), IStorage.StorageNames.Inventory);
        Equipment.AddCard(GameDatabase.Instance.GetCard(401), IStorage.StorageNames.Inventory);
        Equipment.AddCard(GameDatabase.Instance.GetCard(402), IStorage.StorageNames.Inventory);
        Equipment.AddCard(GameDatabase.Instance.GetCard(501), IStorage.StorageNames.Inventory);
        Equipment.AddCard(GameDatabase.Instance.GetCard(502), IStorage.StorageNames.WeaponSlot);
        Equipment.AddCard(GameDatabase.Instance.GetCard(601), IStorage.StorageNames.Inventory);
        Equipment.AddCard(GameDatabase.Instance.GetCard(602), IStorage.StorageNames.Inventory);
    }

    protected override void Death()
    {
        
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Equipment))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CardWeapon cardWeapon;
    private Equipment equipment;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
    }

    void Start()
    {
        //test
        equipment.EquipWeaponCard(cardWeapon);


    }
}

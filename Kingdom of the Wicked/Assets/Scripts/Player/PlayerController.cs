using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Equipment))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private CardWeapon cardWeapon;
    [SerializeField] private CardArmor cardArmor;
    [SerializeField] private CardOther cardOther;
    private Equipment equipment;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
    }

    void Start()
    {
        //test
        equipment.EquipWeaponCard(cardWeapon);
        equipment.EquipArmorCard(cardArmor);
        equipment.EquipOtherCard(cardOther);
    }
}

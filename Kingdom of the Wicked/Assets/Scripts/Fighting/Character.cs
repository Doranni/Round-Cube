using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private StatsValuesSO statsValues;
    private Quaternion originRotation;

    public CharacterStats Stats { get; private set; }
    public CharacterEquipment Equipment { get; private set; }

    void Start()
    {
        Stats = new CharacterStats(statsValues);
        Equipment = new CharacterEquipment(Stats);
        originRotation = transform.rotation;
    }

    public void ResetRotation()
    {
        transform.rotation = originRotation;
    }
}

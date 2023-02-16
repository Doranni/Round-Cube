using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] private StatsValuesSO statsValues;
    private Quaternion originRotation;

    public CharacterStats Stats { get; private set; }
    public CharacterEquipment Equipment { get; private set; }
    public CharacterFighting Fighting { get; private set; }

    void Awake()
    {
        Stats = new CharacterStats(statsValues);
        Equipment = new CharacterEquipment(Stats);
        Fighting = new CharacterFighting(this);
        originRotation = transform.rotation;
    }

    public void ResetRotation()
    {
        transform.rotation = originRotation;
    }
}

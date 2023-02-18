using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected StatsValuesSO statsValues;
    private Quaternion originRotation;

    public CharacterStats Stats { get; private set; }
    public CharacterEquipment Equipment { get; private set; }
    public CharacterDeck Deck { get; private set; }

    protected virtual void Awake()
    {
        Stats = new CharacterStats(statsValues);
        Equipment = new CharacterEquipment(Stats);
        Deck = new CharacterDeck(this);
        originRotation = transform.rotation;
    }

    protected virtual void Start()
    {
        Stats.ChHealth.Died += Death;
    }

    public void ResetRotation()
    {
        transform.rotation = originRotation;
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }
}

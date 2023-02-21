using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected StatsValuesSO statsValues;
    [SerializeField] protected Outline outline;

    public CharacterStats Stats { get; private set; }
    public CharacterEquipment Equipment { get; private set; }
    public CharacterDeck Deck { get; private set; }

    protected virtual void Awake()
    {
        Stats = new CharacterStats(statsValues);
        Equipment = new CharacterEquipment(Stats);
        Deck = new CharacterDeck(this);
    }

    protected virtual void Start()
    {
        Stats.ChHealth.Died += Death;
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    public void Outline(Color color)
    {
        outline.OutlineColor = color;
    }
}

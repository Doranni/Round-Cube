using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected CharacterSO characterSO;
    [SerializeField] protected Outline outline;


    public int Id { get; private set; }
    public string CharacterName { get; private set; }
    public string CharacterDescription { get; private set; }
    public CharacterStats Stats { get; private set; }
    public CharacterEquipment Equipment { get; private set; }
    public CharacterDeck Deck { get; private set; }

    protected virtual void Awake()
    {
        Id = characterSO.id;
        CharacterName = characterSO.characterName;
        CharacterDescription = characterSO.characterDescription;
        Stats = new CharacterStats(characterSO);
        Equipment = new CharacterEquipment(Stats);
        Deck = new CharacterDeck(this);
    }

    protected virtual void Start()
    {
        Stats.ChHealth.Died += Death;
        var data = SavesManager.Instance.Characters.Find(x => x.id == Id);
        if (data != null)
        {
            Stats.ChHealth.SetCurrentHealth(data.health);
        }
    }

    protected virtual void Death()
    {
        Destroy(gameObject);
    }

    public void Outline(Color color)
    {
        outline.OutlineColor = color;
    }

    public void Save()
    {
        SavesManager.Instance.UpdateCharacter(this);
    }
}

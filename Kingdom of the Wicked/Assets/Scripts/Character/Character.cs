using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected CharacterSO characterSO;
    //[SerializeField] protected Outline outline;


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
        Stats = new CharacterStats(this, characterSO);
        Equipment = new CharacterEquipment(this);
        Deck = new CharacterDeck(this);

    }

    protected virtual void Start()
    {
        Stats.ChHealth.Died += Death;
        var data = SavesManager.Instance.Characters.Find(x => x.id == Id);
        if (data != null)
        {
            if (data.isDead)
            {
                Stats.ChHealth.SetIsDead(true);
                Stats.ChHealth.SetCurrentHealth(0);
                Destroy(gameObject);
            }
            foreach (EquippedCard card in data.cards)
            {
                var cardToEquip = GameDatabase.Instance.GetCard(card.id);
                if (cardToEquip != null)
                {
                    Equipment.AddCard(cardToEquip, card.storage, needToSave: false);
                }
            }
            Stats.ChHealth.SetCurrentHealth(data.health);
        }
        else
        {
            foreach (EquippedCardsSO card in characterSO.cards)
            {
                var cardToEquip = GameDatabase.Instance.GetCard(card.cardId);
                if (cardToEquip != null)
                {
                    Equipment.AddCard(cardToEquip, card.storage);
                }
            }
        }
    }

    protected virtual void Death()
    {
        SavesManager.Instance.UpdateCharacter(this);
        Destroy(gameObject);
    }

    public void Outline(Color color)
    {
        //outline.OutlineColor = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int characterId = 0;
    [SerializeField] protected SpriteRenderer bodySpriteRenderer;
    [SerializeField] protected bool modelForFight = false;
    //[SerializeField] protected Outline outline;


    public int CharacterId => characterId;
    public string CharacterName { get; protected set; }
    public string CharacterDescription { get; protected set; }
    public CharacterStats Stats { get; protected set; }
    public CharacterEquipment Equipment { get; protected set; }
    public CharacterDeck Deck { get; protected set; }

    protected virtual void Start()
    {
        if (characterId != 0)
        {
            InitCharacter(characterId);
        }
    }

    public void InitCharacter(int characterId)
    {
        this.characterId = characterId;

        var data = GameDatabase.Instance.Characters[CharacterId];
        CharacterName = data.characterName;
        CharacterDescription = data.characterDescription;
        Stats = new CharacterStats(this, data);
        Equipment = new CharacterEquipment(this);
        Deck = new CharacterDeck(this);
        Stats.ChHealth.Died += Death;
        if (modelForFight)
        {
            bodySpriteRenderer.sprite = data.fightSprite;
        }
        else
        {
            bodySpriteRenderer.sprite = data.boardSprite;
        }

        var saveData = SavesManager.Instance.Characters.Find(x => x.id == CharacterId);
        if (saveData != null)
        {
            if (saveData.isDead)
            {
                Stats.ChHealth.SetIsDead(true);
                Stats.ChHealth.SetCurrentHealth(0);
                Destroy(gameObject);
            }
            foreach (EquippedCard card in saveData.cards)
            {
                var cardToEquip = GameDatabase.Instance.GetCard(card.id);
                if (cardToEquip != null)
                {
                    Equipment.AddCard(cardToEquip, card.storage, needToSave: false);
                }
            }
            Stats.ChHealth.SetCurrentHealth(saveData.health);
        }
        else
        {
            foreach (EquippedCardsSO card in data.cards)
            {
                var cardToEquip = GameDatabase.Instance.GetCard(card.cardId);
                if (cardToEquip != null)
                {
                    Equipment.AddCard(cardToEquip, card.storage);
                }
            }
        }
    }

    protected virtual void Death() { }

    public void Outline(Color color)
    {
        //outline.OutlineColor = color;
    }
}

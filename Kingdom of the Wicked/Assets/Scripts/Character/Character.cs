using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] protected int characterId = 0;
    [SerializeField] protected SpriteRenderer bodySpriteRenderer;
    [SerializeField] protected bool modelForFight = false;
    [SerializeField] protected Animator animator;
    [SerializeField] protected Outline outline;

    protected int anim_death_trigger;

    public int CharacterId => characterId;
    public string CharacterName { get; protected set; }
    public string CharacterDescription { get; protected set; }
    public CharacterStats Stats { get; protected set; }
    public CharacterEquipment Equipment { get; protected set; }

    protected void Awake()
    {
        anim_death_trigger = Animator.StringToHash("Death");
    }

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
        CharacterName = data.CharacterName;
        CharacterDescription = data.CharacterDescription;
        Stats = new CharacterStats(this, data);
        Equipment = new CharacterEquipment(this);
        Stats.ChHealth.Died += Death;
        if (modelForFight)
        {
            bodySpriteRenderer.sprite = data.FightSprite;
        }
        else
        {
            bodySpriteRenderer.sprite = data.BoardSprite;
        }

        var saveData = SavesManager.Instance.Characters.Find(x => x.id == CharacterId);
        if (saveData != null)
        {
            if (saveData.isDead)
            {
                LoadingWhenDead();
            }
            foreach (EquippedCard card in saveData.cards)
            {
                var cardToEquip = GameDatabase.Instance.GetCard(card.id);
                if (cardToEquip != null)
                {
                    switch (cardToEquip.CardType)
                    {
                        case Card.CardsType.Armor:
                            {
                                ((ArmorCard)cardToEquip).SetProtection(card.armor_protection);
                                break;
                            }
                        case Card.CardsType.Shield:
                            {
                                ((ShieldCard)cardToEquip).SetChargesLeft(card.charges);
                                break;
                            }
                        case Card.CardsType.Magic:
                        case Card.CardsType.Potion:
                            {
                                ((ICardBreakable)cardToEquip).SetChargesLeft(card.charges);
                                break;
                            }
                    }
                    Equipment.AddCard(cardToEquip, card.storage, needToSave: false);
                }
            }
            Stats.ChHealth.SetCurrentHealth(saveData.health);
        }
        else
        {
            foreach (EquippedCardsSO card in data.Cards)
            {
                var cardToEquip = GameDatabase.Instance.GetCard(card.CardId);
                if (cardToEquip != null)
                {
                    Equipment.AddCard(cardToEquip, card.Storage, needToSave: false);
                }
            }
        }
    }

    protected virtual void LoadingWhenDead() 
    {
        Stats.ChHealth.SetIsDead(true);
        Stats.ChHealth.SetCurrentHealth(0);
        Destroy(gameObject);
    }

    protected virtual void Death() { }

    public void Outline(Color color)
    {
        outline.OutlineColor = color;
    }
}

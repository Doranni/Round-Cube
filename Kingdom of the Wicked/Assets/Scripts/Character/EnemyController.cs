using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    [SerializeField] private List<int> cards;

    protected override void Awake()
    {
        if (characterSO != null)
        {
            base.Awake();
        }
    }

    protected override void Start()
    {
        if (characterSO != null)
        {
            base.Start();
            foreach (int cardId in cards)
            {
                var card = GameDatabase.Instance.GetCard(cardId);
                Equipment.EquipCard(card);
            }
        }
    }

    public void InitEnemie(CharacterSO characterSO)
    {
        this.characterSO = characterSO;
        Awake();
        Start();
    }

    public (Card card, bool useOnYourself) ChooseCard()
    {
        return (Deck.BattleCardsUsable[0], false);
    }
}

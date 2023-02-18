using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    [SerializeField] private List<int> cards;

    protected override void Start()
    {
        base.Start();
        foreach(int cardId in cards)
        {
            var card = GameDatabase.Instance.GetCard(cardId);
            Equipment.EquipCard(card);
        }
    }

    public (Card card, bool useOnYourself) ChooseCard()
    {
        return (Deck.BattleCardsUsable[0], false);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : Character
{
    public (Card card, bool useOnYourself) ChooseCard()
    {
        return (Deck.BattleCardsUsable[0], false);
    }

    protected override void Death()
    {
        SavesManager.Instance.UpdateCharacter(this);
        Destroy(gameObject);
    }
}

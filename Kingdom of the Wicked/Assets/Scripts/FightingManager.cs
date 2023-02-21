using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FightingManager : Singleton<FightingManager>
{
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;

    public PlayerController Player => player;
    public EnemyController Enemy => enemy;
    public Character CurrentTurn { get; private set; }

    public override void Awake()
    {
        base.Awake();
        
        StartFight();
    }

    public void StartFight()
    {
        CurrentTurn = player;
    }

    private void EndFight()
    {
        GameManager.Instance.EndFight();
    }

    public void NextTurn()
    {
        Player.Stats.ExecuteEffects();
        Enemy.Stats.ExecuteEffects();
        CurrentTurn.Deck.UnselectCards();
        if (Player.Stats.ChHealth.IsDead)
        {
            Debug.Log($"Player died");
            EndFight();
            return;
        }
        if (Enemy.Stats.ChHealth.IsDead)
        {
            Debug.Log($"Enemie died");
            EndFight();
            return;
        }
        if (CurrentTurn.gameObject == Player.gameObject)
        {
            CurrentTurn = Enemy;
            StartCoroutine(EnemyFightRoutine());
        }
        else
        {
            CurrentTurn = Player;
        }
    }

    private IEnumerator EnemyFightRoutine()
    {
        (Card card, bool useOnYourself) = Enemy.ChooseCard();
        yield return new WaitForSeconds(0.5f);
        Enemy.Deck.SelectBattleCard(card.InstanceId);
        yield return new WaitForSeconds(0.5f);
        if (useOnYourself)
        {
            ((IUsable)card).Use(Enemy);
        }
        else
        {
            ((IUsable)card).Use(Player);
        }
        NextTurn();
    }

    public bool TrySetTarget(Character target)
    {
        if (IsTarget(target))
        {
            ((IUsable)Player.Deck.SelectedBattleCard).Use(Enemy);
            NextTurn();
            return true;
        }
        return false;
    }

    public bool IsTarget(Character target)
    {
        if (CurrentTurn == Player && Player.Deck.SelectedBattleCard != null)
        {
            return true;
        }
        return false;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingManager : Singleton<FightingManager>
{
    public enum State
    {
        notInFight,
        playersTurn,
        enemiesTurn
    }

    [SerializeField] private CameraController cameraController;
    [SerializeField] private PlayerController player;
    public PlayerController Player => player;
    public EnemyController Enemy { get; private set; }
    public State CurrentState { get; private set; }

    public event Action FightStarted, FightEnded;

    public override void Awake()
    {
        base.Awake();
        CurrentState = State.notInFight;
    }

    public void StartFight(EnemyController enemy, Transform cameraPoint)
    {
        Enemy = enemy;
        cameraController.SwitchToFightingCamera(cameraPoint);
        Player.Deck.UpdateCards();
        Enemy.Deck.UpdateCards();
        EnemieUI.Instance.StartFight(Enemy);
        Player.transform.rotation = cameraPoint.transform.rotation;
        Vector3 enemmieRot = cameraPoint.transform.rotation.eulerAngles;
        enemmieRot.y += 180;
        Enemy.transform.rotation = Quaternion.Euler(enemmieRot);
        CurrentState = State.playersTurn;

        FightStarted?.Invoke();
    }

    private void EndFight()
    {
        Player.Deck.UnselectCards();
        Enemy.Deck.UnselectCards();
        EnemieUI.Instance.EndFight();
        player.ResetRotation();
        Enemy.ResetRotation();
        Enemy = null;
        cameraController.SwitchToFollowCamera();
        CurrentState = State.notInFight;
        FightEnded?.Invoke();
    }

    public void NextTurn()
    {
        Player.Stats.ExecuteEffects();
        Enemy.Stats.ExecuteEffects();
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
        switch (CurrentState)
        {
            case State.playersTurn:
                {
                    Player.Deck.UnselectCards();
                    CurrentState = State.enemiesTurn;
                    StartCoroutine(EnemyFightRoutine());
                    break;
                }
            case State.enemiesTurn:
                {
                    Enemy.Deck.UnselectCards();
                    CurrentState = State.playersTurn;
                    break;
                }
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
        if (CurrentState == State.playersTurn && target.gameObject == Enemy.gameObject
            && Player.Deck.SelectedBattleCard != null)
        {
            return true;
        }
        return false;
    }
}

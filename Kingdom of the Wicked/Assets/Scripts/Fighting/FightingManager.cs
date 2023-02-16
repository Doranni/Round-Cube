using System;
using System.Collections.Generic;
using UnityEngine;

public class FightingManager : Singleton<FightingManager>
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Character player;
    public Character Player => player;
    public Character Enemie { get; private set; }
    public Character CurrentTurn { get; private set; }

    private EnemieController enemieController;

    public event Action FightStarted, FightEnded;
    public event Action<Character> StartTurn;

    public override void Awake()
    {
        base.Awake();
    }

    public void StartFight(Character enemie, Transform cameraPoint)
    {
        Enemie = enemie;
        cameraController.SetFightingCameraTarget(cameraPoint);
        Player.Fighting.UpdateCards();
        Enemie.Fighting.UpdateCards();
        enemieController = Enemie.GetComponent<EnemieController>();
        enemieController.StartFight();
        EnemieUI.Instance.StartFight(Enemie);


        Player.transform.rotation = cameraPoint.transform.rotation;
        Vector3 enemmieRot = cameraPoint.transform.rotation.eulerAngles;
        enemmieRot.y += 180;
        Enemie.transform.rotation = Quaternion.Euler(enemmieRot);
        CurrentTurn = player;

        FightStarted?.Invoke();
        StartTurn?.Invoke(CurrentTurn);
    }

    private void EndFight()
    {
        EnemieUI.Instance.EndFight();
        player.ResetRotation();
        Enemie.ResetRotation();
        enemieController.EndFight();
        Enemie = null;
        FightEnded?.Invoke();
    }

    public void NextTurn()
    {
        Player.Stats.ExecuteEffects();
        Enemie.Stats.ExecuteEffects();
        if (Player.Stats.ChHealth.IsDead)
        {
            Debug.Log($"Player died");
            EndFight();
            return;
        }
        if (Enemie.Stats.ChHealth.IsDead)
        {
            Debug.Log($"Enemie died");
            EndFight();
            return;
        }
        CurrentTurn = (CurrentTurn == player) ? Enemie : player;
        Debug.Log($"{CurrentTurn.name}'s turn:");
        StartTurn?.Invoke(CurrentTurn);
    }
}

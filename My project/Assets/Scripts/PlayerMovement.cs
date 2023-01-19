using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class PlayerMovement : MonoBehaviour
{
   public enum Status
    {
        onDestinationNode,
        moving,
        onBetweenNode,
        waitingNodeChosen
    }

    [SerializeField] private float timeDelay_diceRolled = 0.3f;

    private NavMeshAgent navMeshAgent;

    private int index_currentNodePos;
    private Coroutine diceRolledRoutine;
    private Status status;

    public event Action OnMoveFinished;

    void Start()
    {
        //InputManager.Instance.OnMouseClick_canceled += MouseClicked;

        DiceRoller.Instance.OnDiceRolled += DiceRolled;
        DiceRoller.Instance.OnDiceResChanged += DiceResChanged;

        navMeshAgent = GetComponent<NavMeshAgent>();
        MoveToStart();
    }

    private void Update()
    {
        switch (status)
        {
            case Status.moving:
                {
                    if (navMeshAgent.remainingDistance == 0)
                    {
                        status = Status.onBetweenNode;
                        OnMoveFinished?.Invoke();
                    }
                    break;
                }
        }
    }

    private void DiceRolled(int value)
    {
        diceRolledRoutine = StartCoroutine(DiceRolledRoutine(value));
    }

    private IEnumerator DiceRolledRoutine(int value)
    {
        yield return new WaitForSeconds(timeDelay_diceRolled);
        DiceResChanged(value);
    }

    private void DiceResChanged(int value)
    {
        if (value > 0)
        {
            Move();
        }
        else
        {
            status = Status.onDestinationNode;
            // TODO: Unlock way back
        }
    }

    private void MouseClicked(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        Debug.Log("Mouse Clicked");
    }

    private void MoveToStart()
    {
        index_currentNodePos = Map.Instance.Index_start;
        navMeshAgent.Warp(Map.Instance.MapNodes[index_currentNodePos].transform.position);
        status = Status.onDestinationNode;
    }

    private void Move()
    {
        var links = Map.Instance.AvailableLinks(index_currentNodePos);
        if (links.Count == 1)
        {
            index_currentNodePos = links[0].DestinationNode.Index;
            // TODO: Lock way back 
            navMeshAgent.SetPath(links[0].Path);
            status = Status.moving;
        }
        else
        {
            status = Status.waitingNodeChosen;
        }
    }

    private void OnDestroy()
    {
        //InputManager.Instance.OnMouseClick_performed -= MouseClicked;

        DiceRoller.Instance.OnDiceRolled -= DiceRolled;
        DiceRoller.Instance.OnDiceResChanged -= DiceResChanged;
    }
}

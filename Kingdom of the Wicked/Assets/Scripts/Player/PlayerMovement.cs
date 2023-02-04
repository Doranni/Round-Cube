using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
   public enum MoveStatus
    {
        onDestinationNode,
        moving,
        onBetweenNode,
        waitingNodeChosen
    }

    [SerializeField] private float speed = 10, posOffset = 0.4f, timeDelay_diceRolled = 0.2f;

    public int NodeIndex { get; private set; }

    private CharacterController characterController;

    public MoveStatus MStatus { get; private set; }

    private float yOffset;
    private float yGravity;

    private int destNodeIndex;
    private List<Vector3> destPoints = new();
    private int passedDestPoints;
    private (bool wasLocked, NodeLink link) lockedLink = (false, null);
    private Vector3 moveVector;

    private Coroutine diceRolledRoutine;

    public event Action OnStepStarted, OnStepFinished;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        DiceRoller.Instance.OnDiceRolled += DiceRolled;
        DiceRoller.Instance.OnDiceResChanged += DiceResChanged;

        yOffset = GetComponent<Collider>().bounds.extents.y;
        yGravity = Physics.gravity.y;
        
        MoveToStart();
    }

    private void Update()
    {
        switch (MStatus)
        {
            case MoveStatus.moving:
                {
                    if (Mathf.Abs(transform.position.x - destPoints[passedDestPoints].x) < posOffset
                        && Mathf.Abs(transform.position.z - destPoints[passedDestPoints].z) < posOffset)
                    {
                        passedDestPoints++;
                    }
                    if (passedDestPoints == destPoints.Count)
                    {
                        destPoints.Clear();
                        MStatus = MoveStatus.onBetweenNode;
                        NodeIndex = destNodeIndex;
                        OnStepFinished?.Invoke();
                        return;
                    }
                    moveVector = (destPoints[passedDestPoints] - transform.position).normalized;
                    moveVector.y = yGravity;
                    characterController.Move(moveVector * speed * Time.deltaTime);
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
            MStatus = MoveStatus.onDestinationNode;
            if (lockedLink.wasLocked)
            {
                lockedLink.link.SetIsAvailable(true);
                lockedLink.wasLocked = false;
                lockedLink.link = null;
            }
            Map.Instance.MapNodes[NodeIndex].NEvent.Visit();
        }
    }

    private void MoveToStart()
    {
        destNodeIndex = Map.Instance.Index_start;
        characterController.enabled = false;
        transform.position = Map.Instance.MapNodes[destNodeIndex].StayPoint
            + Vector3.up * yOffset;
        characterController.enabled = true;
        characterController.Move(Vector3.up * yGravity);
        MStatus = MoveStatus.onDestinationNode;
        NodeIndex = destNodeIndex;
        Map.Instance.MapNodes[NodeIndex].NEvent.Visit();
    }

    private void Move()
    {
        var links = Map.Instance.GetAvailableLinks(NodeIndex);
        if (links.Count == 1)
        {
            SetDestination(links[0]);
        }
        else
        {
            MStatus = MoveStatus.waitingNodeChosen;
        }
    }

    public void ChooseMoveNode(MapNode node)
    {
        if (Map.Instance.IsNodeReachable(NodeIndex, node.Index))
        {
            var link = Map.Instance.MapNodes[NodeIndex].Links[node.Index];
            SetDestination(link);
        }
    }

    public void ConsiderMoveNode(MapNode node)
    {
        if (Map.Instance.IsNodeReachable(NodeIndex, node.Index))
        {
            // TODO: to outline this way
            Debug.Log("We can go there!");
        }

    }

    private void SetDestination(NodeLink link)
    {
        if (lockedLink.wasLocked)
        {
            lockedLink.link.SetIsAvailable(true);
            lockedLink.wasLocked = false;
            lockedLink.link = null;
        }
        if (Map.Instance.MapNodes[link.NodeTo.Index].Links.ContainsKey(NodeIndex))
        {
            var linkBack = Map.Instance.MapNodes[link.NodeTo.Index].Links[NodeIndex];
            linkBack.SetIsAvailable(false);
            lockedLink = (true, linkBack);
        }
        destNodeIndex = link.NodeTo.Index;
        for (int i = 1; i < link.PathPoints.Count; i++)
        {
            destPoints.Add(link.PathPoints[i]);
        }
        passedDestPoints = 0;
        OnStepStarted?.Invoke();
        MStatus = MoveStatus.moving;
    }

    private void OnDestroy()
    {
        DiceRoller.Instance.OnDiceRolled -= DiceRolled;
        DiceRoller.Instance.OnDiceResChanged -= DiceResChanged;
    }
}

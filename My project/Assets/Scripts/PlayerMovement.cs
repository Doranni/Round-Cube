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

    [SerializeField] private float speed = 8, posOffset = 0.1f, timeDelay_diceRolled = 0.3f;

    private CharacterController characterController;

    private MoveStatus moveStatus;
    private int destNodeIndex;
    private List<Vector3> destPoints = new();
    private int passedDestPoints;
    private float yOffset;
    private float yGravity;
    private List<NodeLink> movedLinks = new();

    private Coroutine diceRolledRoutine;

    public event Action OnMoveFinished;

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

    private void FixedUpdate()
    {
        switch (moveStatus)
        {
            case MoveStatus.moving:
                {
                    if (passedDestPoints == destPoints.Count)
                    {
                        destPoints.Clear();
                        moveStatus = MoveStatus.onBetweenNode;
                        OnMoveFinished?.Invoke();
                    }
                    else
                    {
                        var vector = (destPoints[passedDestPoints] - transform.position).normalized;
                        vector.y = yGravity;
                        characterController.Move(vector * speed * Time.deltaTime);

                        if (Mathf.Abs(transform.position.x - destPoints[passedDestPoints].x) < posOffset 
                            && Mathf.Abs(transform.position.z - destPoints[passedDestPoints].z) < posOffset)
                        {
                            passedDestPoints++;
                        }
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
            moveStatus = MoveStatus.onDestinationNode;
            for (int i = 0; i < movedLinks.Count; i++)
            {
                movedLinks[i].SetIsAvailable(true);
            }
            movedLinks.Clear();
        }
    }

    private void MoveToStart()
    {
        destNodeIndex = Map.Instance.Index_start;
        characterController.enabled = false;
        transform.position = Map.Instance.MapNodes[destNodeIndex].transform.position
            + Vector3.up * yOffset;
        characterController.enabled = true;
        characterController.Move(Vector3.up * yGravity);
        moveStatus = MoveStatus.onDestinationNode;
    }

    private void Move()
    {
        var links = Map.Instance.AvailableLinks(destNodeIndex);
        if (links.Count == 1)
        {
            if (links[0].direction == NodeLink.Direction.forward)
            {
                destNodeIndex = links[0].link.DestNodeForward.node.Index;
            }
            else
            {
                destNodeIndex = links[0].link.DestNodeBackward.node.Index;
            }
            links[0].link.SetIsAvailable(false);
            movedLinks.Add(links[0].link);
            SetDestPoints(links[0]);
            moveStatus = MoveStatus.moving;
        }
        else
        {
            moveStatus = MoveStatus.waitingNodeChosen;
        }
    }

    private void SetDestPoints((NodeLink link, NodeLink.Direction direction) link)
    {
        if (link.direction == NodeLink.Direction.forward)
        {
            for (int i = 1; i < link.link.PathPoints.Count; i++)
            {
                destPoints.Add(link.link.PathPoints[i]);
            }
        }
        else
        {
            for (int i = link.link.PathPoints.Count - 2; i >= 0; i--)
            {
                destPoints.Add(link.link.PathPoints[i]);
            }
        }
        passedDestPoints = 0;
    }

    private void OnDestroy()
    {
        DiceRoller.Instance.OnDiceRolled -= DiceRolled;
        DiceRoller.Instance.OnDiceResChanged -= DiceResChanged;
    }
}

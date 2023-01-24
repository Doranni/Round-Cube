using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeLink : MonoBehaviour
{
    [SerializeField] private MapNode nodeFrom;
    [SerializeField] private MapNode nodeTo;
    [SerializeField] private bool isWayForwardOpen;
    [SerializeField] private bool isWayBackOpen;
    [SerializeField] private List<Transform> pathPoints;

    public enum Direction
    {
        forward,
        backward
    }

    public event Action OnNodeToOpen, OnNodeFromOpen,
        OnNodeToClosed, OnNodeFromClosed;

    public (MapNode node, bool isOpen) NodeTo { get; private set; }
    public (MapNode node, bool isOpen) NodeFrom { get; private set; }
    public List<Vector3> PathPoints { get; private set; }
    public bool IsAvailable { get; private set; }

    private void Awake()
    {
        NodeTo = (nodeTo, isWayForwardOpen);
        NodeFrom = (nodeFrom, isWayBackOpen);
        IsAvailable = true;
        InitPathPoints();
    }

    private void InitPathPoints()
    {
        PathPoints = new();
        PathPoints.Add(nodeFrom.StayPoint);
        for (int i = 0; i < pathPoints.Count; i++)
        {
            PathPoints.Add(pathPoints[i].position);
        }
        PathPoints.Add(nodeTo.StayPoint);
    }

    public void SetNodeIsOpen(Direction direction, bool isOpen)
    {
        switch (direction)
        {
            case Direction.forward:
                {
                    NodeTo = (NodeTo.node, isOpen);
                    if (isOpen)
                    {
                        OnNodeToOpen?.Invoke();
                    }
                    else
                    {
                        OnNodeToClosed?.Invoke();
                    }
                    break;
                }
            case Direction.backward:
                {
                    NodeFrom = (NodeFrom.node, isOpen);
                    if (isOpen)
                    {
                        OnNodeFromOpen?.Invoke();
                    }
                    else
                    {
                        OnNodeFromClosed?.Invoke();
                    }
                    break;
                }
        }
    }

    public void SetIsAvailable(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}

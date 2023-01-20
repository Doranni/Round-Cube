using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeLink : MonoBehaviour
{
    [SerializeField] private MapNode destNodeForward;
    [SerializeField] private bool isForwardOpen;
    [SerializeField] private MapNode destNodeBackward;
    [SerializeField] private bool isBackwardOpen;
    [SerializeField] private List<Transform> pathPoints;

    public enum Direction
    {
        forward,
        backward
    }

    public event Action OnDestNodeForwardOpen, OnDestNodeBackwardOpen,
        OnDestNodeForwardClosed, OnDestNodeBackwardClosed;

    public (MapNode node, bool isOpen) DestNodeForward { get; private set; }
    public (MapNode node, bool isOpen) DestNodeBackward { get; private set; }
    public List<Vector3> PathPoints { get; private set; }
    public bool IsAvailable { get; private set; }

    private void Awake()
    {
        DestNodeForward = (destNodeForward, isForwardOpen);
        DestNodeBackward = (destNodeBackward, isBackwardOpen);
        IsAvailable = true;
        InitPathPoints();
    }

    private void InitPathPoints()
    {
        PathPoints = new();
        for (int i = 0; i < pathPoints.Count; i++)
        {
            PathPoints.Add(pathPoints[i].position);
        }
    }

    public void SetNodeIsOpen(Direction direction, bool isOpen)
    {
        switch (direction)
        {
            case Direction.forward:
                {
                    DestNodeForward = (DestNodeForward.node, isOpen);
                    if (isOpen)
                    {
                        OnDestNodeForwardOpen?.Invoke();
                    }
                    else
                    {
                        OnDestNodeForwardClosed?.Invoke();
                    }
                    break;
                }
            case Direction.backward:
                {
                    DestNodeBackward = (DestNodeBackward.node, isOpen);
                    if (isOpen)
                    {
                        OnDestNodeBackwardOpen?.Invoke();
                    }
                    else
                    {
                        OnDestNodeBackwardClosed?.Invoke();
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

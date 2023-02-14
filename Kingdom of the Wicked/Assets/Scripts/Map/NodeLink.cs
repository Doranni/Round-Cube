using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeLink : MonoBehaviour
{
    [SerializeField] private MapNode nodeFrom;
    [SerializeField] private MapNode nodeTo;
    [SerializeField] private bool isWayOpen;
    [SerializeField] private List<Transform> pathPoints;

    public event Action WayWasOpened, WayWasClosed;

    public MapNode NodeTo => nodeTo;
    public MapNode NodeFrom => nodeFrom;
    public bool IsWayOpen => isWayOpen;
    public List<Vector3> PathPoints { get; private set; }
    public bool IsAvailable { get; private set; }

    private void Awake()
    {
        IsAvailable = true;
        InitPathPoints();
    }

    private void InitPathPoints()
    {
        PathPoints = new(pathPoints.Count + 2);
        PathPoints.Add(nodeFrom.AbowePoint);
        for (int i = 0; i < pathPoints.Count; i++)
        {
            PathPoints.Add(pathPoints[i].position);
        }
        PathPoints.Add(nodeTo.AbowePoint);
    }

    public void SetWayIsOpen(bool isOpen)
    {
        var prevVal = isWayOpen;
        isWayOpen = isOpen;
        if (!prevVal && isWayOpen)
        {
            WayWasOpened?.Invoke();
        }
        else if (prevVal && !isWayOpen)
        {
            WayWasClosed?.Invoke();
        }
    }

    public void SetIsAvailable(bool isAvailable)
    {
        IsAvailable = isAvailable;
    }
}

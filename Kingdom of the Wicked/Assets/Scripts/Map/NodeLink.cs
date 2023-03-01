using System;
using System.Collections.Generic;
using UnityEngine;

public class NodeLink
{
    public event Action WayWasOpened, WayWasClosed;

    public int NextMapNodeId { get; private set; }
    public bool IsWayOpen { get; private set; }
    public List<Vector3> PathPoints { get; private set; }

    public NodeLink(int prevMapNodeId, int nextMapNodeId, bool isWayOpen, Vector3[] betweenPathPoints)
    {
        NextMapNodeId = nextMapNodeId;
        IsWayOpen = isWayOpen;

        PathPoints = new(betweenPathPoints.Length + 3);
        PathPoints.Add(Map.Instance.MapNodes[prevMapNodeId].AbowePoint);
        for (int i = 0; i < betweenPathPoints.Length; i++)
        {
            PathPoints.Add(betweenPathPoints[i]);
        }
        PathPoints.Add(Map.Instance.MapNodes[nextMapNodeId].AbowePoint);
        PathPoints.Add(Map.Instance.MapNodes[nextMapNodeId].StayPoint);
    }

    public void SetWayIsOpen(bool isOpen)
    {
        var prevVal = IsWayOpen;
        IsWayOpen = isOpen;
        if (!prevVal && IsWayOpen)
        {
            WayWasOpened?.Invoke();
        }
        else if (prevVal && !IsWayOpen)
        {
            WayWasClosed?.Invoke();
        }
    }
}

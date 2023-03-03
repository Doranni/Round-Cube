using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   public enum MoveStatus
    {
        onNode,
        moving,
    }


    [SerializeField] private Collider plCollider;
    [SerializeField] private float speed = 10;

    public int CurrentNodeId { get; private set; }
    public MoveStatus MStatus { get; private set; }

    private float yOffset;

    private List<Vector3> destPoints = new();
    private int passedDestPoints;
    private Vector3 movePosition;

    void Start()
    {
        yOffset = plCollider.bounds.extents.y - plCollider.bounds.center.y;
        MoveToStart();
    }

    private void Update()
    {
        switch (MStatus)
        {
            case MoveStatus.moving:
                {
                    if (transform.position == movePosition)
                    {
                        passedDestPoints++;
                        if (passedDestPoints == destPoints.Count)
                        {
                            destPoints.Clear();
                            MStatus = MoveStatus.onNode;
                            SavesManager.Instance.UpdatePlayerPos(CurrentNodeId);
                            Map.Instance.MapNodes[CurrentNodeId].Visit();
                            return;
                        }
                        movePosition = destPoints[passedDestPoints] + Vector3.up * yOffset;
                    }
                    transform.position = Vector3.MoveTowards(transform.position, movePosition, 
                        Time.deltaTime * speed);
                    break;
                }
        }
    }

    private void MoveToStart()
    {
        CurrentNodeId = SavesManager.Instance.PlayerNodeIndex;
        transform.position = Map.Instance.MapNodes[CurrentNodeId].StayPoint
            + Vector3.up * yOffset;
        MStatus = MoveStatus.onNode;
        Map.Instance.MapNodes[CurrentNodeId].Visit();
    }

    public bool TrySetMoveNode(MapNode node)
    {
        if (MStatus == MoveStatus.onNode && Map.Instance.IsNodeReachable(CurrentNodeId, node.MapNodeId))
        {
            var link = Map.Instance.MapNodes[CurrentNodeId].Links[node.MapNodeId];
            SetDestination(link);
            return true;
        }
        return false;
    }

    public bool IsNodeReachable(MapNode node)
    {
        return (MStatus == MoveStatus.onNode && Map.Instance.IsNodeReachable(CurrentNodeId, node.MapNodeId));
    }

    private void SetDestination(NodeLink link)
    {
        CurrentNodeId = link.NextMapNodeId;
        for (int i = 0; i < link.PathPoints.Count; i++)
        {
            destPoints.Add(link.PathPoints[i]);
        }
        passedDestPoints = 0;
        movePosition = destPoints[passedDestPoints] + Vector3.up * yOffset;
        MStatus = MoveStatus.moving;
    }
}

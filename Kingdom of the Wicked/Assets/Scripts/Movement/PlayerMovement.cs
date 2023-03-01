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

    public int NodeIndex { get; private set; }
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
                            SavesManager.Instance.UpdatePlayerPos(NodeIndex);
                            Map.Instance.MapNodes[NodeIndex].Visit();
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
        NodeIndex = SavesManager.Instance.PlayerNodeIndex;
        transform.position = Map.Instance.MapNodes[NodeIndex].StayPoint
            + Vector3.up * yOffset;
        MStatus = MoveStatus.onNode;
        Map.Instance.MapNodes[NodeIndex].Visit();
    }

    public bool TrySetMoveNode(MapNode node)
    {
        if (MStatus == MoveStatus.onNode && Map.Instance.IsNodeReachable(NodeIndex, node.MapNodeId))
        {
            var link = Map.Instance.MapNodes[NodeIndex].Links[node.MapNodeId];
            SetDestination(link);
            return true;
        }
        return false;
    }

    public bool IsNodeReachable(MapNode node)
    {
        return (MStatus == MoveStatus.onNode && Map.Instance.IsNodeReachable(NodeIndex, node.MapNodeId));
    }

    private void SetDestination(NodeLink link)
    {
        NodeIndex = link.NextMapNodeId;
        for (int i = 0; i < link.PathPoints.Count; i++)
        {
            destPoints.Add(link.PathPoints[i]);
        }
        passedDestPoints = 0;
        movePosition = destPoints[passedDestPoints] + Vector3.up * yOffset;
        MStatus = MoveStatus.moving;
    }
}

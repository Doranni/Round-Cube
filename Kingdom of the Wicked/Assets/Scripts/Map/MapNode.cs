using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField] private Transform stayPoint;
    [SerializeField] private bool isStart = false;
    [SerializeField] private NodeEvent nEvent;

    public Vector3 StayPoint => stayPoint.transform.position;
    public Vector3 AbowePoint => StayPoint + (Vector3.up * GameManager.Instance.PlMovementHeight);
    public bool IsStart => isStart;
    public int Index { get; private set; }
    public Dictionary<int, NodeLink> Links { get; private set; }
    public NodeEvent NEvent => nEvent;

    private void Awake()
    {
        Links = new();
    }

    public void SetIndex(int index)
    {
        Index = index;
    }
    public void SetIsStart(bool isStart)
    {
        this.isStart = isStart;
    }

    public void AddLink(NodeLink link)
    {
        Links.Add(link.NodeTo.Index, link);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField] private Transform stayPoint;
    [SerializeField] private bool isStart = false;
    [SerializeField] private MeshRenderer meshRenderer;

    public Vector3 StayPoint => stayPoint.transform.position;
    public bool IsStart => isStart;
    public int Index { get; private set; }
    public Dictionary<int, NodeLink> Links { get; private set; }
    public NodeEvent NEvent { get; private set; }

    private void Awake()
    {
        Links = new();
        NEvent = new(meshRenderer.material);
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
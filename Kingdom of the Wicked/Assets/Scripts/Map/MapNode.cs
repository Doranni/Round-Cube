using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField] private bool isStart = false;
    public bool IsStart => isStart;
    public int Index { get; private set; }
    public List<NodeLink> Links { get; private set; }

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
        Links.Add(link);
    }
}
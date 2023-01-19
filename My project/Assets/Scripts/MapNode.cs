using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MapNode : MonoBehaviour
{
    [SerializeField] private List<NodeLink> links;
    [SerializeField] private bool isStart = false;
    private int index;

    public bool IsRoot => isStart;
    public List<NodeLink> Links => links;
    public int Index => index;

    public void SetIndex(int index)
    {
        this.index = index;
    }
}

[System.Serializable]
public class NodeLink
{
    [SerializeField] private bool isOpen;
    [SerializeField] private MapNode destinationNode;
    private NavMeshPath path;

    public bool IsOpen => isOpen;
    public MapNode DestinationNode => destinationNode;
    public NavMeshPath Path => path;

    public void SetPath(NavMeshPath path)
    {
        this.path = path;
    }
}

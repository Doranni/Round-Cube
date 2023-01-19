using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private MapNode node;

    public bool IsOpen => isOpen;
    public MapNode Node => node;
}

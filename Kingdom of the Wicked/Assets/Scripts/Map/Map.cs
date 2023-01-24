using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    public List<MapNode> MapNodes { get; private set; }
    public List<NodeLink> Links { get; private set; }
    public int Index_start { get; private set; }

    private void Start()
    {
        InitMap();
        Debug.Log($"mapNodes count - {MapNodes.Count}");
    }

    private void InitMap()
    {
        MapNodes = new();
        GetComponentsInChildren(MapNodes);

        Links = new();
        GetComponentsInChildren(Links);

        for (int i = 0; i < MapNodes.Count; i++)
        {
            MapNodes[i].SetIndex(i);
        }
        Index_start = GetStartIndex();
        SetLinks();
    }

    private int GetStartIndex()
    {
        int res = -1;
        for (int i = 0; i < MapNodes.Count; i++)
        {
            if (MapNodes[i].IsStart)
            {
                if (res == -1)
                {
                    res = i;
                }
                else
                {
                    MapNodes[i].SetIsStart(false);
                }
            }
        }
        if (res == -1)
        {
            MapNodes[0].SetIsStart(true);
            return 0;
        }
        else
        {
            return res;
        }
    }

    private void SetLinks()
    {
        for (int i = 0; i < Links.Count; i++)
        {
            MapNodes[Links[i].NodeTo.node.Index].AddLink(Links[i]);
            MapNodes[Links[i].NodeFrom.node.Index].AddLink(Links[i]);
        }
    }

    public List<(NodeLink link, NodeLink.Direction direction)> AvailableLinks(int index)
    {
        List<(NodeLink link, NodeLink.Direction type)> links = new();
        for ( int i = 0; i < MapNodes[index].Links.Count; i++)
        {
            if (MapNodes[index].Equals(MapNodes[index].Links[i].NodeFrom.node)
                && MapNodes[index].Links[i].NodeTo.isOpen
                && MapNodes[index].Links[i].IsAvailable)
            {
                links.Add((MapNodes[index].Links[i], NodeLink.Direction.forward));
            }
            else if (MapNodes[index].Equals(MapNodes[index].Links[i].NodeTo.node)
                && MapNodes[index].Links[i].NodeFrom.isOpen
                && MapNodes[index].Links[i].IsAvailable)
            {
                links.Add((MapNodes[index].Links[i], NodeLink.Direction.backward));
            }
        }
        return links;
    }

    public bool IsNodeReachable(int currentIndex, int nextNodeIndex)
    {
        var links = AvailableLinks(currentIndex);
        for (int i = 0; i < links.Count; i++)
        {
            if (links[i].direction == NodeLink.Direction.forward
                && links[i].link.NodeTo.node.Index == nextNodeIndex)
            {
                return true;
            }
            if (links[i].direction == NodeLink.Direction.backward
                && links[i].link.NodeFrom.node.Index == nextNodeIndex)
            {
                return true;
            }
        }
        return false;
    }

    public (NodeLink link, NodeLink.Direction direction) GetNodeLink(int node1Index, int node2Index)
    {
        var links = AvailableLinks(node1Index);
        for (int i = 0; i < links.Count; i++)
        {
            if (links[i].direction == NodeLink.Direction.forward
                && links[i].link.NodeTo.node.Index == node2Index)
            {
                return links[i];
            }
            if (links[i].direction == NodeLink.Direction.backward
                && links[i].link.NodeFrom.node.Index == node2Index)
            {
                return links[i];
            }
        }
        return (null, NodeLink.Direction.forward);
    }
}

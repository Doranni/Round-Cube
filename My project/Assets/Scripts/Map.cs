using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

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
            MapNodes[Links[i].DestNodeForward.node.Index].AddLink(Links[i]);
            MapNodes[Links[i].DestNodeBackward.node.Index].AddLink(Links[i]);
        }
    }

    public List<(NodeLink link, NodeLink.Direction direction)> AvailableLinks(int index)
    {
        List<(NodeLink link, NodeLink.Direction type)> links = new();
        for ( int i = 0; i < MapNodes[index].Links.Count; i++)
        {
            if (MapNodes[index].Equals(MapNodes[index].Links[i].DestNodeBackward.node)
                && MapNodes[index].Links[i].DestNodeForward.isOpen
                && MapNodes[index].Links[i].IsAvailable)
            {
                links.Add((MapNodes[index].Links[i], NodeLink.Direction.forward));
            }
            else if (MapNodes[index].Equals(MapNodes[index].Links[i].DestNodeForward.node)
                && MapNodes[index].Links[i].DestNodeBackward.isOpen
                && MapNodes[index].Links[i].IsAvailable)
            {
                links.Add((MapNodes[index].Links[i], NodeLink.Direction.backward));
            }
        }
        return links;
    }
}

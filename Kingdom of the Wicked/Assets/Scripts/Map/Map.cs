using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    [SerializeField] private MapNode[] mapNodes;
    [SerializeField] private NodeLink[] links;

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
        MapNodes = new(mapNodes);
        Links = new(links);
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
            MapNodes[Links[i].NodeFrom.Index].AddLink(Links[i]);
        }
    }

    public List<NodeLink> GetAvailableLinks(int index)
    {
        List<NodeLink> links = new();
        foreach(KeyValuePair<int, NodeLink> link in MapNodes[index].Links)
        {
            if (link.Value.IsWayOpen
                && link.Value.IsAvailable)
            {
                links.Add(link.Value);
            }
        }
        return links;
    }

    public bool IsNodeReachable(int currentIndex, int nextNodeIndex)
    {
        if (MapNodes[currentIndex].Links.ContainsKey(nextNodeIndex)
            && MapNodes[currentIndex].Links[nextNodeIndex].IsWayOpen
            && MapNodes[currentIndex].Links[nextNodeIndex].IsAvailable)
        {
            return true;
        }
        return false;
    }
}

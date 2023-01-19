using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    public List<MapNode> MapNodes { get; private set; }
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
        for (int i = 0; i < MapNodes.Count; i++)
        {
            MapNodes[i].SetIndex(i);
        }
        Index_start = GetStartIndex();
    }

    private int GetStartIndex()
    {
        for (int i = 0; i < MapNodes.Count; i++)
        {
            if (MapNodes[i].IsRoot)
            {
                return i;
            }
        }
        return 0;
    }

    public List<MapNode> AvailableLinks(int index)
    {
        List<MapNode> links = new();
        for ( int i = 0; i < MapNodes[index].Links.Count; i++)
        {
            if (MapNodes[index].Links[i].IsOpen)
            {
                links.Add(MapNodes[index].Links[i].Node);
            }
        }
        return links;
    }
}

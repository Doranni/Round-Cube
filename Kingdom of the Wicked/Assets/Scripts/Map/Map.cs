using System.Collections.Generic;
using UnityEngine;

public class Map : Singleton<Map>
{
    [SerializeField] private MapSO map;
    [SerializeField] private MapNode[] mapNodes;

    public Dictionary<int, MapNode> MapNodes { get; private set; }
    public int Index_start { get; private set; }

    private void Start()
    {
        InitMap();
        Debug.Log($"mapNodes count - {MapNodes.Count}");
    }

    private void InitMap()
    {
        MapNodes = new(mapNodes.Length);
        for (int i = 0; i < mapNodes.Length; i++)
        {
            MapNodes.Add(mapNodes[i].MapNodeId, mapNodes[i]);
            MapNodes[mapNodes[i].MapNodeId].Load();
        }

        Index_start = map.startMapNodeId;

        for (int i = 0; i < map.mapNodes.Length; i++)
        {
            for (int j = 0; j < map.mapNodes[i].links.Length; j++)
            {
                MapNodes[map.mapNodes[i].mapNodeId].AddLink(map.mapNodes[i].links[j]);
            }
        }
    }

    public bool IsNodeReachable(int currentNodeId, int nextNodeId)
    {
        if (MapNodes[currentNodeId].Links.ContainsKey(nextNodeId)
            && MapNodes[currentNodeId].Links[nextNodeId].IsWayOpen)
        {
            return true;
        }
        return false;
    }
}

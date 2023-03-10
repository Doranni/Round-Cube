using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Map")]
public class MapSO : ScriptableObject
{
    [SerializeField] private MapNodeSO[] mapNodes;
    [SerializeField] private int startMapNodeId;

    public MapNodeSO[] MapNodes => mapNodes;
    public int StartMapNodeId => startMapNodeId;
}

[Serializable]
public class MapNodeSO
{
    [SerializeField] private int mapNodeId;
    [SerializeField] private MapLinkSO[] links;

    public int MapNodeId => mapNodeId;
    public MapLinkSO[] Links => links;
}

[Serializable]
public class MapLinkSO
{
    [SerializeField] private int nextMapNodeId;
    [SerializeField] private bool isWayOpen;
    [SerializeField] private Vector3[] betweenPathPoints;

    public int NextMapNodeId => nextMapNodeId;
    public bool IsWayOpen => isWayOpen;
    public Vector3[] BetweenPathPoints => betweenPathPoints;
}

using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Map", menuName = "Map")]
public class MapSO : ScriptableObject
{
    public MapNodeSO[] mapNodes;
    public int startMapNodeId;
}

[Serializable]
public class MapNodeSO
{
    public int mapNodeId;
    public MapLinkSO[] links;
}

[Serializable]
public class MapLinkSO
{
    public int nextMapNodeId;
    public bool isWayOpen;
    public Vector3[] betweenPathPoints;
}

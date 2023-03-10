using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField] private int mapNodeId;
    [SerializeField] private Transform stayPoint;
    [SerializeField] private List<ChestController> chests;
    [SerializeField] private List<NPC> npcs;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private Outline outline;

    public int MapNodeId => mapNodeId;
    public Vector3 StayPoint { get; private set; }
    public Vector3 AbowePoint { get; private set; }
    public Dictionary<int, NodeLink> Links { get; private set; }
    public List<ChestController> Chests => chests;
    public List<NPC> Npcs => npcs;
    public EnemyController Enemy => enemy;
    public bool IsVisited { get; protected set; }

    private void Awake()
    {
        Links = new();
        IsVisited = false;
        StayPoint = stayPoint.transform.position;
        AbowePoint = StayPoint + (Vector3.up * GameManager.Instance.PlMovementHeight);
    }

    public void Load()
    {
        var data = SavesManager.Instance.MapNodes.Find(x => x.nodeId == MapNodeId);
        if (data != null)
        {
            
        }
    }

    public void AddLink(MapLinkSO link)
    {
        Links.Add(link.NextMapNodeId, new NodeLink(mapNodeId, link.NextMapNodeId, link.IsWayOpen, 
            link.BetweenPathPoints));
    }

    public virtual void Visit()
    {
        if (!IsVisited)
        {
            foreach (ChestController chest in chests)
            {
                chest.Unlock();
            }
            foreach (NPC npc in npcs)
            {
                npc.Unlock();
            }
            IsVisited = true;
            //SavesManager.Instance.UpdateMapNode(MapNodeId, true);
        }
        if (enemy != null && !enemy.Stats.ChHealth.IsDead)
        {
            SavesManager.Instance.SetEnemieForFight(enemy.CharacterId);
            GameManager.Instance.StartFight();
        }
    }

    public void Outline(Color color)
    {
        outline.OutlineColor = color;
    }
}
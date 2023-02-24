using System.Collections.Generic;
using UnityEngine;

public class MapNode : MonoBehaviour
{
    [SerializeField] private Transform stayPoint;
    [SerializeField] private bool isStart = false;
    [SerializeField] private List<Chest> chests;
    [SerializeField] private List<NPC> npcs;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private Outline outline;

    public Vector3 StayPoint => stayPoint.transform.position;
    public Vector3 AbowePoint => StayPoint + (Vector3.up * GameManager.Instance.PlMovementHeight);
    public bool IsStart => isStart;
    public int Index { get; private set; }
    public Dictionary<int, NodeLink> Links { get; private set; }
    public List<Chest> Chests => chests;
    public List<NPC> Npcs => npcs;
    public EnemyController Enemy => enemy;
    public bool IsVisited { get; protected set; }

    private void Awake()
    {
        Links = new();
        IsVisited = false;
    }

    public void SetIndex(int index)
    {
        Index = index;
        var data = SavesManager.Instance.MapNodes.Find(x => x.index == Index);
        if (data != null)
        {
            IsVisited = data.isVisited;
        }
    }
    public void SetIsStart(bool isStart)
    {
        this.isStart = isStart;
    }

    public void AddLink(NodeLink link)
    {
        Links.Add(link.NodeTo.Index, link);
    }

    public virtual void Visit()
    {
        if (IsVisited)
        {
            return;
        }
        foreach (Chest chest in chests)
        {
            chest.Unlock();
        }
        foreach (NPC npc in npcs)
        {
            npc.Unlock();
        }
        IsVisited = true;
        SavesManager.Instance.UpdateMapNode(Index, true);
        if (enemy != null)
        {
            SavesManager.Instance.SetEnemieForFight(enemy.Id);
            GameManager.Instance.StartFight(Index);
        }
    }

    public void Outline(Color color)
    {
        outline.OutlineColor = color;
    }
}
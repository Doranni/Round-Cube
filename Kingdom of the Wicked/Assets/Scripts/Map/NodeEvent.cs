using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent : MonoBehaviour
{
    [SerializeField] private Outline outline;
    [SerializeField] private List<Chest> chests;
    [SerializeField] private List<NPC> npcs;
    [SerializeField] private EnemyController enemy;
    [SerializeField] private Transform cameraPoint;

    public List<Chest> Chests => chests;
    public List<NPC> Npcs => npcs;
    public EnemyController Enemy => enemy;
    public Transform CameraPoint => cameraPoint;
    public bool IsVisited { get; protected set; }

    protected virtual void Awake()
    {
        IsVisited = false;
    }

    public virtual void Visit()
    {
        foreach (Chest chest in chests)
        {
            chest.Unlock();
        }
        foreach (NPC npc in npcs)
        {
            npc.Unlock();
        }
        IsVisited = true;
        if (enemy != null && cameraPoint != null)
        {
            FightingManager.Instance.StartFight(enemy, cameraPoint);
        }
    }

    public void Outline(Color color)
    {
        outline.OutlineColor = color;
    }
}



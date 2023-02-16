using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent : MonoBehaviour
{
    [SerializeField] protected MeshRenderer meshRenderer;
    [SerializeField] protected List<Chest> chests;
    [SerializeField] protected List<NPC> npcs;
    [SerializeField] protected Character enemie;
    [SerializeField] protected Transform cameraPoint;

    public List<Chest> Chests => chests;
    public List<NPC> Npcs => npcs;
    public Character Enemie => enemie;
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
        meshRenderer.material.color = Color.gray;
        if (enemie != null && cameraPoint != null)
        {
            FightingManager.Instance.StartFight(enemie, cameraPoint);
        }
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent : MonoBehaviour
{
    public enum NodeEventType
    {
        neutral,
        fighting
    }

    [SerializeField] protected List<IInteractable> objects;
    [SerializeField] protected MeshRenderer meshRenderer;

    public List<IInteractable> Objects => objects;
    public bool IsVisited { get; protected set; }

    protected virtual void Awake()
    {
        IsVisited = false;
    }

    public virtual void Visit()
    {
        IsVisited = true;
        meshRenderer.material.color = Color.gray;
    }
}



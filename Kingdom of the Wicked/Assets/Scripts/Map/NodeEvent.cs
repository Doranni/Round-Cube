using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeEvent
{
    public bool IsVisited { get; private set; }

    private Material material;

    public NodeEvent(Material material)
    {
        IsVisited = false;
        this.material = material;
    }

    public void Visit()
    {
        IsVisited = true;
        material.color = Color.gray;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public bool IsLocked { get; private set; }

    private void Awake()
    {
        IsLocked = true;
    }

    public void Interact()
    {
        Debug.Log($"Interaction with NPC {name}");
    }

    public void Unlock()
    {
        IsLocked = false;
        Debug.Log($"NPC {name} was unlocked");
    }
}

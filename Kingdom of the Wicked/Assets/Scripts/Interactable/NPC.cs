using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, IInteractable
{
    public bool isLocked { get; private set; }

    private void Awake()
    {
        isLocked = true;
    }

    public void Interact()
    {
        Debug.Log($"Interaction with NPC {name}");
    }

    public void Unlock()
    {
        isLocked = false;
        Debug.Log($"NPC {name} was unlocked");
    }
}

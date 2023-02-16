using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IInteractable
{
    public bool isLocked { get; private set; }

    private void Awake()
    {
        isLocked = true;
    }

    public void Interact()
    {
        Debug.Log($"Interaction with Chest {name}");
    }

    public void Unlock()
    {
        isLocked = false;
        Debug.Log($"Chest {name} was unlocked");
    }
}

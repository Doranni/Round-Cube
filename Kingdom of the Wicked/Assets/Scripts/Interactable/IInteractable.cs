using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool isLocked { get; }
    public void Interact();
    public void Unlock();
}

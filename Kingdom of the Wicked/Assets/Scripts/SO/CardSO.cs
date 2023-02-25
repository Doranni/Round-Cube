using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CardSO : ScriptableObject
{
    public int id;
    public string cardName;
    public string description;
    public int duration;
    public Texture image;
}


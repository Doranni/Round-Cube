using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingNodeEvent : NodeEvent
{
    [SerializeField] private Character enemie;
    [SerializeField] private Transform cameraPoint;

    public Character Enemie => enemie;
    public Transform CameraPoint => cameraPoint;

    public override void Visit()
    {
        base.Visit();
        FightingManager.Instance.StartFight(this);
    }
}

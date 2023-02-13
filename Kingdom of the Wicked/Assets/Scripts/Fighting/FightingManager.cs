using System;
using System.Collections.Generic;
using UnityEngine;

public class FightingManager : Singleton<FightingManager>
{
    [SerializeField] private CameraController cameraController;
    [SerializeField] private Character player;
    public Character Player => player;
    public Character Enemie { get; private set; }

    public event Action<bool> FightStarted;

    public void StartFight(FightingNodeEvent node)
    {
        Enemie = node.Enemie;
        cameraController.SetFightingCameraTarget(node.CameraPoint);
        
        Player.transform.rotation = node.CameraPoint.transform.rotation;
        Vector3 enemmieRot = node.CameraPoint.transform.rotation.eulerAngles;
        enemmieRot.y += 180;
        Enemie.transform.rotation = Quaternion.Euler(enemmieRot);

        FightStarted?.Invoke(true);
    }

    private void EndFight()
    {
        FightStarted?.Invoke(false);
        player.ResetRotation();
        Enemie.ResetRotation();
        Enemie = null;
    }
}

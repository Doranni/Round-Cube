using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MapInput : Singleton<MapInput>
{
    [SerializeField] private PlayerMovement pMovement;

    public void MapNode_Clicked(MapNode node)
    {
        if (pMovement.MStatus == PlayerMovement.MoveStatus.waitingNodeChosen)
        {
            pMovement.ChooseMoveNode(node);
        }
    }

    public void MapNode_PointerEnter(MapNode node)
    {
        if (pMovement.MStatus == PlayerMovement.MoveStatus.waitingNodeChosen)
        {
            pMovement.ConsiderMoveNode(node);
        }
    }

    public void MapNode_PointerExit(MapNode node)
    {
        
    }

    public void Character_Clicked(Character character)
    {
        FightingManager.Instance.TrySetTarget(character);
    }

    public void Character_PointerEnter(Character character)
    {

    }

    public void Character_PointerExit(Character character)
    {

    }
}


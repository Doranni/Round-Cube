using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardInputManager : Singleton<BoardInputManager>
{
    [SerializeField] private PlayerMovement pMovement;
    [SerializeField] private Color outline_objectAvailable, outline_fightTargetAvailable, 
        outline_unavailable;
    private Color outline_default = new Color(0, 0, 0, 0);

    public void MapNode_Clicked(MapNode node)
    {
        if (pMovement.TrySetMoveNode(node))
        {
            node.Outline(outline_default);
        }
    }

    public void MapNode_PointerEnter(MapNode node)
    {
        if (pMovement.IsNodeReachable(node))
        {
            node.Outline(outline_objectAvailable);
        }
        else if (node.MapNodeId != pMovement.CurrentNodeId)
        {
            node.Outline(outline_unavailable);
        }
    }

    public void MapNode_PointerExit(MapNode node)
    {
        node.Outline(outline_default);
    }

    public void Character_Clicked(Character character)
    {
        //FightingManager.Instance.TrySetTarget(character);
    }

    public void Character_PointerEnter(Character character)
    {

    }

    public void Character_PointerExit(Character character)
    {

    }

    public void Chest_Clicked(ChestController chest)
    {
        if (pMovement.CurrentNodeId == chest.MapNodeId)
        {
            chest.Interact();
        }
    }

    public void Chest_PointerEnter(ChestController chest)
    {
        if (chest.CanBeOpen() && pMovement.CurrentNodeId == chest.MapNodeId)
        {
            chest.Outline(outline_objectAvailable);
        }
        else 
        {
            chest.Outline(outline_unavailable);
        }
    }

    public void Chest_PointerExit(ChestController chest)
    {
        chest.Outline(outline_default);
    }
}


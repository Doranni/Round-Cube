using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BoardInputManager : Singleton<BoardInputManager>
{
    [SerializeField] private PlayerMovement pMovement;
    [SerializeField] private Color outline_mapNodeAvailable, outline_fightTargetAvailable, 
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
            node.Outline(outline_mapNodeAvailable);
        }
        else if (node.Index != pMovement.NodeIndex)
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
        FightingManager.Instance.TrySetTarget(character);
    }

    public void Character_PointerEnter(Character character)
    {

    }

    public void Character_PointerExit(Character character)
    {

    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FightingTargetInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Character character;
    [SerializeField] private Color outline_targetAvailable;
    private Color outline_default = new Color(0, 0, 0, 0);

    public void OnPointerDown(PointerEventData eventData)
    {
        if (FightingManager.Instance.TrySetTarget(character))
        {
            character.Outline(outline_default);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (FightingManager.Instance.IsTarget(character))
        {
            character.Outline(outline_targetAvailable);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        character.Outline(outline_default);
    }
}

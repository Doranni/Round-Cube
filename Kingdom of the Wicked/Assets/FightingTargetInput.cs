using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FightingTargetInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Character character;
    [SerializeField] private Color outline_targetAttack, outline_TargetHeal;
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
        var (isTarget, effect) = FightingManager.Instance.IsTarget(character);
        if (isTarget)
        {
            if (effect == Card.CardEffectType.Harm)
            {
                character.Outline(outline_targetAttack);
            }
            else
            {
                character.Outline(outline_TargetHeal);
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        character.Outline(outline_default);
    }
}

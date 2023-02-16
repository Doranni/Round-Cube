using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Character))]
public class CharacterInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Character character;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (GameManager.Instance.State == GameManager.GameState.fighting 
            && FightingManager.Instance.CurrentTurn == FightingManager.Instance.Player
            && FightingManager.Instance.Enemie == character)
        {
            FightingManager.Instance.Player.Fighting.UseBattleCard(character);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        
    }
}
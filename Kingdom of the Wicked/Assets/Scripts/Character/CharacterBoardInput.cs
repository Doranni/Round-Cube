using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Character))]
public class CharacterBoardInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Character character;

    public void OnPointerDown(PointerEventData eventData)
    {
        BoardInputManager.Instance.Character_Clicked(character);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BoardInputManager.Instance.Character_PointerEnter(character);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BoardInputManager.Instance.Character_PointerExit(character);
    }
}
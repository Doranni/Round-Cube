using UnityEngine;
using UnityEngine.EventSystems;

public class ChestInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private ChestController chest;

    public void OnPointerDown(PointerEventData eventData)
    {
        BoardInputManager.Instance.Chest_Clicked(chest);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BoardInputManager.Instance.Chest_PointerEnter(chest);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BoardInputManager.Instance.Chest_PointerExit(chest);
    }
}

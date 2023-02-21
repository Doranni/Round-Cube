using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MapNode))]
public class MapNodeInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MapNode mapNode;

    public void OnPointerDown(PointerEventData eventData)
    {
        BoardInputManager.Instance.MapNode_Clicked(mapNode);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        BoardInputManager.Instance.MapNode_PointerEnter(mapNode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        BoardInputManager.Instance.MapNode_PointerExit(mapNode);
    }
}

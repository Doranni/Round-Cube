using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(MapNode))]
public class MapNodeInput : MonoBehaviour, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private MapNode mapNode;

    public void OnPointerDown(PointerEventData eventData)
    {
        MapInput.Instance.MapNode_Clicked(mapNode);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MapInput.Instance.MapNode_PointerEnter(mapNode);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MapInput.Instance.MapNode_PointerExit(mapNode);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragCardManipulator : PointerManipulator
{
    private Vector2 cardStartPos;
    private Vector3 pointerStartPos;
    private Vector2 dragRangeMin, dragRangeMax;
    private Vector2 cardSize;
    private Card.CardsType type;

    public DragCardManipulator(VisualElement card, Vector2 cardSize, Card.CardsType type)
    {
        target = card;
        this.cardSize = cardSize;
        this.type = type;
    }

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(PointerDownHandler);
        target.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.RegisterCallback<PointerUpEvent>(PointerUpHandler);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(PointerDownHandler);
        target.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);
        target.UnregisterCallback<PointerUpEvent>(PointerUpHandler);
    }

    private void PointerDownHandler(PointerDownEvent evt)
    {
        dragRangeMin = target.WorldToLocal(DragCardManager.Instance.DragRangeMin);
        dragRangeMax = target.WorldToLocal(new Vector2(
            DragCardManager.Instance.DragRangeMax.x - cardSize.x,
            DragCardManager.Instance.DragRangeMax.y - cardSize.y));
        pointerStartPos = evt.position;
        cardStartPos = target.transform.position;
        target.CapturePointer(evt.pointerId);
    }

    private void PointerUpHandler(PointerUpEvent evt)
    {
        var slots = DragCardManager.Instance.GetAvailableSlots(type);
        for (int i = 0; i < slots.Count; i++)
        {
            if (OverlapsTarget(slots[i]))
            {
                target.ReleasePointer(evt.pointerId);
                return;
            }
        }
        target.ReleasePointer(evt.pointerId);
        target.transform.position = cardStartPos;
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (target.HasPointerCapture(evt.pointerId))
        {
            Vector3 pointerDelta = evt.position - pointerStartPos;

            target.transform.position = new Vector2(
            Mathf.Clamp(cardStartPos.x + pointerDelta.x, dragRangeMin.x, dragRangeMax.x),
            Mathf.Clamp(cardStartPos.y + pointerDelta.y, dragRangeMin.y, dragRangeMax.y));
        }
    }

    private bool OverlapsTarget(VisualElement slot)
    {
        return target.worldBound.Overlaps(slot.worldBound);
    }
}
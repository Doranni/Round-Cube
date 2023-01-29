using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropManipulator : PointerManipulator
{
    private Vector2 cardStartPos;
    private Vector3 pointerStartPos;
    private Vector2 dragRangeMin, dragRangeMax;
    private Vector2 cardSize;
    private Card.CardsType type;
    private Card card;
    private Storage.StorageNames prevSlotName;

    public DragAndDropManipulator(VisualElement cardUI, Vector2 cardSize, Card card,
        Storage.StorageNames prevSlotName)
    {
        target = cardUI;
        this.cardSize = cardSize;
        this.card = card;
        this.type = card.CardType;
        this.prevSlotName = prevSlotName;
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
        target.BringToFront();
        dragRangeMin = target.WorldToLocal(GameManager.Instance.DragRangeMin);
        dragRangeMax = target.WorldToLocal(new Vector2(
            GameManager.Instance.DragRangeMax.x - cardSize.x,
            GameManager.Instance.DragRangeMax.y - cardSize.y));
        pointerStartPos = evt.position;
        cardStartPos = target.transform.position;
        target.CapturePointer(evt.pointerId);
    }

    private void PointerUpHandler(PointerUpEvent evt)
    {
        var storages = EquipmentUI.Instance.GetAvailableStorages(type);
        for (int i = 0; i < storages.Count; i++)
        {
            if (OverlapsTarget(storages[i].storageUI))
            {
                target.ReleasePointer(evt.pointerId);
                EquipmentUI.Instance.CardWasMoved(prevSlotName, storages[i].storageName);
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

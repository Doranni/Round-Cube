using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropController : Singleton<DragAndDropController>
{
    private CardToDragVisualElement cardToDrag;
    private VisualElement root;

    public CardVE target;
    private Vector2 targetStartPos;
    private Vector3 pointerStartPos;

    private Vector2 dragRange;

    public bool IsDragging => target != null;

    public void Init(VisualElement root)
    {
        cardToDrag = new CardToDragVisualElement();
        this.root = root;
        this.root.Add(cardToDrag);
    }

    public void AddTarget(PointerDownEvent evt, CardVE target)
    {
        cardToDrag.Init(target.CardData);

        this.target = target;
        targetStartPos = cardToDrag.WorldToLocal(target.LocalToWorld(target.transform.position));
        target.style.display = DisplayStyle.None;

        pointerStartPos = evt.position;

        cardToDrag.transform.position = targetStartPos;
        cardToDrag.style.display = DisplayStyle.Flex;
        
        cardToDrag.CapturePointer(evt.pointerId);
        cardToDrag.RegisterCallback<PointerUpEvent>(PointerUpEventHandler);
        cardToDrag.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);

        BoardSceneUI.Instance.OpenSlotsHolders(target.CardData, true);
        dragRange = new Vector2(cardToDrag.panel.visualTree.worldBound.width - 80,
            cardToDrag.panel.visualTree.worldBound.height - 120);
    }

    private void PointerUpEventHandler(PointerUpEvent evt)
    {
        var storages = BoardSceneUI.Instance.GetAvailableStorages();
        List<IStorage.StorageNames> overlapStorages = new();
        for (int i = 0; i < storages.Count; i++)
        {
            if (OverlapsCard(storages[i]))
            {
                overlapStorages.Add(storages[i].Storage.StorageName);
            }
        }
        BoardSceneUI.Instance.CardWasMoved(target.CardData, target.Storage, overlapStorages);
        ResetTarget(evt.pointerId);
    }

    private bool OverlapsCard(VisualElement storage)
    {
        return cardToDrag.worldBound.Overlaps(storage.worldBound);
    }

    private void ResetTarget(int pointerId)
    {
        cardToDrag.ReleasePointer(pointerId);
        cardToDrag.UnregisterCallback<PointerUpEvent>(PointerUpEventHandler);
        cardToDrag.UnregisterCallback<PointerMoveEvent>(PointerMoveHandler);

        if (target != null)
        {
            target.style.display = DisplayStyle.Flex;
        }
        target = null;
        cardToDrag.Clean();
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (cardToDrag.HasPointerCapture(evt.pointerId))
        {
            Vector3 pointerDelta = evt.position - pointerStartPos;

            cardToDrag.transform.position = new Vector2(
            Mathf.Clamp(targetStartPos.x + pointerDelta.x, 0, dragRange.x),
            Mathf.Clamp(targetStartPos.y + pointerDelta.y, 0, dragRange.y));
        }
    }
}

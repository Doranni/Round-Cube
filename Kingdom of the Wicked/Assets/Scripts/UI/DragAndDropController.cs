using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropController : Singleton<DragAndDropController>
{
    private CardToDragVisualElement cardToDrag;
    private VisualElement root;

    private CardVisualElement target;
    private Vector2 targetStartPos;
    private Vector3 pointerStartPos;

    public void Init(VisualElement root)
    {
        cardToDrag = new CardToDragVisualElement();
        this.root = root;
        this.root.Add(cardToDrag);
    }

    public void AddTarget(PointerDownEvent evt, CardVisualElement target)
    {
        cardToDrag.Init(target.CardInfo);

        this.target = target;
        targetStartPos = cardToDrag.WorldToLocal(target.LocalToWorld(target.transform.position));
        target.style.display = DisplayStyle.None;

        pointerStartPos = evt.position;

        cardToDrag.transform.position = targetStartPos;
        cardToDrag.style.display = DisplayStyle.Flex;
        
        cardToDrag.CapturePointer(evt.pointerId);
        cardToDrag.RegisterCallback<PointerUpEvent>(PointerUpEventHandler);
        cardToDrag.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
    }

    private void PointerUpEventHandler(PointerUpEvent evt)
    {
        var storages = EquipmentUI.Instance.GetAvailableStorages(target.CardInfo.CardType);
        List<IStorage.StorageNames> overlapStorages = new();
        for (int i = 0; i < storages.Count; i++)
        {
            if (OverlapsCard(storages[i].storageUI))
            {
                overlapStorages.Add(storages[i].storageName);
            }
        }
        EquipmentUI.Instance.CardWasMoved(target.CardInfo, target.Storage, overlapStorages);
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
            Mathf.Clamp(targetStartPos.x + pointerDelta.x, GameManager.Instance.DragRangeMin.x,
                GameManager.Instance.DragRangeMax.x),
            Mathf.Clamp(targetStartPos.y + pointerDelta.y, GameManager.Instance.DragRangeMin.y,
                GameManager.Instance.DragRangeMax.y));
        }
    }
}

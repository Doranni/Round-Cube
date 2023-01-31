using UnityEngine;
using UnityEngine.UIElements;

public class DragAndDropController : Singleton<DragAndDropController>
{
    private VisualElement cardToDrag, root;

    private VisualElement targetVE;
    private Card targetCard;
    private Vector2 targetStartPos;
    private IStorage.StorageNames targetPrevStorage;

    private Vector3 pointerStartPos;

    public void SetCardToDrag(VisualElement cardVE, VisualElement root)
    {
        cardToDrag = cardVE;
        this.root = root;
        this.root.Add(cardVE);
        cardToDrag.style.position = Position.Absolute;
        cardToDrag.style.display = DisplayStyle.None;
    }

    public void AddTarget(PointerDownEvent evt, (VisualElement targetVE, Card card,
        IStorage.StorageNames prevStorageName) target)
    {
        UIManager.Instance.StyleCardToDrag(cardToDrag, target.card);

        targetVE = target.targetVE;
        targetCard = target.card;
        targetStartPos = cardToDrag.WorldToLocal(target.targetVE.LocalToWorld(target.targetVE.transform.position));
        targetPrevStorage = target.prevStorageName;
        target.targetVE.style.display = DisplayStyle.None;

        pointerStartPos = evt.position;

        cardToDrag.transform.position = targetStartPos;
        cardToDrag.style.display = DisplayStyle.Flex;
        
        cardToDrag.CapturePointer(evt.pointerId);
        cardToDrag.RegisterCallback<PointerUpEvent>(PointerUpEventHandler);
        cardToDrag.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
    }

    private void PointerUpEventHandler(PointerUpEvent evt)
    {
        var storages = EquipmentUI.Instance.GetAvailableStorages(targetCard.CardType);
        for (int i = 0; i < storages.Count; i++)
        {
            if (OverlapsCard(storages[i].storageUI))
            {
                EquipmentUI.Instance.CardWasMoved(targetVE, targetCard, targetPrevStorage, storages[i].storageName);
                ResetTarget(evt.pointerId);
                return;
            }
        }
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

        if (targetVE != null)
        {
            targetVE.style.display = DisplayStyle.Flex;
        }
        targetVE = null;
        targetCard = null;

        cardToDrag.style.display = DisplayStyle.None;
        cardToDrag.transform.position = Vector3.zero;
        cardToDrag.style.top = 0;
        cardToDrag.style.left = 0;
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (cardToDrag.HasPointerCapture(evt.pointerId))
        {
            Vector3 pointerDelta = evt.position - pointerStartPos;

            cardToDrag.transform.position = new Vector2(
            Mathf.Clamp(targetStartPos.x + pointerDelta.x, UIManager.Instance.DragRangeMin.x,
                UIManager.Instance.DragRangeMax.x - UIManager.Instance.CardToDragSizeOffset.x),
            Mathf.Clamp(targetStartPos.y + pointerDelta.y, UIManager.Instance.DragRangeMin.y,
                UIManager.Instance.DragRangeMax.y - UIManager.Instance.CardToDragSizeOffset.y));
            Debug.Log("cardToDrag.transform.position - " + cardToDrag.transform.position);
        }
    }
}

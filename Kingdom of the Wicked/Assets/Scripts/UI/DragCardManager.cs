using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DragCardManager : Singleton<DragCardManager>
{
    private CardToDragVisualElement cardToDrag;
    private VisualElement dragCardPanel;

    public CardVE target;
    private Vector2 targetStartPos;
    private Vector3 pointerStartPos;

    private Vector2 dragRange;

    public bool IsDragging { get; private set; }

    const string k_dragCardPanel = "CardPanel";

    private void Start()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        dragCardPanel = rootElement.Q(k_dragCardPanel);
        cardToDrag = new CardToDragVisualElement();
        dragCardPanel.Add(cardToDrag);
        IsDragging = false;
    }

    public void AddTarget(PointerDownEvent evt, CardVE target)
    {
        cardToDrag.Init(target.CardData);

        this.target = target;
        targetStartPos = cardToDrag.WorldToLocal(target.LocalToWorld(target.transform.position));
        pointerStartPos = evt.position;
        
        cardToDrag.CapturePointer(evt.pointerId);
        cardToDrag.RegisterCallback<PointerUpEvent>(PointerUpEventHandler);
        cardToDrag.RegisterCallback<PointerMoveEvent>(PointerMoveHandler);
        
        dragRange = new Vector2(cardToDrag.panel.visualTree.worldBound.width - 80,
            cardToDrag.panel.visualTree.worldBound.height - 120);
    }

    private void PointerUpEventHandler(PointerUpEvent evt)
    {
        if (IsDragging)
        {
            List<StorageVE> storages;
            if (LoadSceneManager.Instance.State == LoadSceneManager.LoadState.Board)
            {
                storages = BoardSceneUI.Instance.GetAvailableStorages();
            }
            else
            {
                storages = new();
            }
            List<IStorage.StorageNames> overlapStorages = new();
            for (int i = 0; i < storages.Count; i++)
            {
                if (OverlapsCard(storages[i]))
                {
                    overlapStorages.Add(storages[i].Storage.StorageName);
                }
            }
            if (LoadSceneManager.Instance.State == LoadSceneManager.LoadState.Board)
            {
                BoardSceneUI.Instance.CardWasMoved(target.CardData, target.Storage, overlapStorages);
            }
            else
            {

            }
        }
        else
        {
            if ((LoadSceneManager.Instance.State == LoadSceneManager.LoadState.Board
                && target.CardData.CardType == Card.CardsType.Potion)
                || (LoadSceneManager.Instance.State == LoadSceneManager.LoadState.Fighting
                && FightingManager.Instance.CurrentTurn == FightingManager.Turn.Player
                && target.CardData is ICardUsable))
            {
                target.CardData.Owner.Equipment.SelectCard(target.CardData);
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

        if (target != null)
        {
            target.CardData.HideCard(false);
        }
        target = null;
        cardToDrag.Clean();
        IsDragging = false;
    }

    private void PointerMoveHandler(PointerMoveEvent evt)
    {
        if (!IsDragging)
        {
            target.CardData.HideCard(true);
            cardToDrag.style.display = DisplayStyle.Flex;
            cardToDrag.transform.position = targetStartPos;
            BoardSceneUI.Instance.OpenSlotsHolders(target.CardData, true);
            IsDragging = true;
        }
        else
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
}

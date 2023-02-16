using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class StorageVE : VisualElement
{
    public IStorage Storage { get; private set; }
    public bool IsActive { get; private set; }
    public List<CardVE> Cards { get; private set; }

    public virtual void Init(IStorage storage)
    {
        Storage = storage;
        IsActive = true;
        Cards = new(storage.Cards.Capacity);
        storage.CardsChanged += Update;
    }

    public virtual void Update()
    {
        Clear();
        Cards.Clear();
        for (int i = 0; i < Storage.Cards.Count; i++)
        {
            var card = new CardVE(Storage.Cards[i], Storage.StorageName);
            Cards.Add(card);
            Add(card);
        }
    }

    public void SetIsActive(bool isActive)
    {
        IsActive = isActive;
    }
}

public class SlotVE : StorageVE
{
    public new class UxmlFactory : UxmlFactory<SlotVE> { }

    public SlotsHolderVE SlotHolder { get; private set; }

    public override void Init(IStorage slot)
    {
        base.Init(slot);
        SetSize();
        pickingMode = PickingMode.Ignore;
    }

    public void SetSlotHolder(SlotsHolderVE slotsHolder)
    {
        SlotHolder = slotsHolder;
    }

    private void SetSize()
    {
        var size = GameManager.Instance.CardSize_slot;
        style.width = size.x;
        style.height = size.y;
    }

    public override void Update()
    {
        base.Update();
        if (SlotHolder != null)
        {
            SlotHolder.Update();
        }
    }
}

public class InventoryVE : StorageVE
{
    public new class UxmlFactory : UxmlFactory<InventoryVE> { }

    public InventoryVE() { }
}

public class SlotsHolderVE : VisualElement
{
    public new class UxmlFactory : UxmlFactory<SlotsHolderVE> { }

    public SlotsHolder SlotHolderData { get; private set; }
    public List<(SlotVE closedSlot, SlotVE openSlot)> Slots { get; private set; }
    public bool IsOpen { get; private set; }
    private int equippedCardsEmount, firstCardIndex;

    private VisualElement slotsClosed, slotsOpen;
    private const string k_slotsClosed = "Slots_Closed";
    private const string k_slotsOpen = "Slots_Open";

    public void Init(SlotsHolder slotHolder)
    {
        hierarchy.Add(GameManager.Instance.SlotsHolderAsset.CloneTree());
        slotsClosed = this.Q(k_slotsClosed);
        slotsOpen = this.Q(k_slotsOpen);

        SlotHolderData = slotHolder;
        Slots = new();

        for(int i = 0; i < slotHolder.Slots.Count; i++)
        {
            var (closed, open) = (new SlotVE(), new SlotVE());

            closed.Init(slotHolder.Slots[i]);
            open.Init(slotHolder.Slots[i]);

            closed.AddToClassList("slot_closed");
            open.AddToClassList("background");
            open.AddToClassList("slot_open");

            slotsClosed.Add(closed);
            slotsOpen.Add(open);

            Slots.Add((closed, open));
        }

        slotsOpen.style.left = -(slotHolder.Slots.Count - 1) * 45 - 45;
        ToggleSlotPanel(false);
    }

    public void Update()
    {
        int equippedCardsEmountRes = 0;
        firstCardIndex = -1;
        for (int i = 0; i < Slots.Count; i++)
        {
            if (Slots[i].closedSlot.Cards.Count > 0)
            {
                equippedCardsEmountRes++;
                if (firstCardIndex == -1)
                {
                    firstCardIndex = i;
                    Slots[i].closedSlot.Cards[0].style.display = DisplayStyle.Flex;
                }
                else
                {
                    Slots[i].closedSlot.Cards[0].style.display = DisplayStyle.None;
                }
            }
        }
        equippedCardsEmount = equippedCardsEmountRes;
    }

    public void ToggleSlotPanel(bool open)
    {
        IsOpen = open;
        if (IsOpen)
        {
            slotsOpen.style.display = DisplayStyle.Flex;
        }
        else
        {
            slotsOpen.style.display = DisplayStyle.None;
        }
    }

    protected override void ExecuteDefaultActionAtTarget(EventBase evt)
    {
        base.ExecuteDefaultActionAtTarget(evt);

        if(evt.eventTypeId == PointerEnterEvent.TypeId())
        {
            if (!DragAndDropController.Instance.IsDragging)
            {
                ToggleSlotPanel(true);
            }
        }
        if (evt.eventTypeId == PointerLeaveEvent.TypeId())
        {
            if (!DragAndDropController.Instance.IsDragging)
            {
                ToggleSlotPanel(false);
            }
        }
    }
}
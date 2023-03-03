using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour, IInteractable
{
    [SerializeField] private int chestId;
    [SerializeField] private int mapNodeId;
    [SerializeField] Animator animator;
    [SerializeField] private Outline outline;

    public int ChestId => chestId;
    public int MapNodeId => mapNodeId;
    public Chest ChestStorage { get; private set; }
    public bool IsLocked { get; private set; }
    public bool IsEmpty { get; private set; }

    private int anim_open_trigger, anim_empty_trigger;

    private void Awake()
    {
        anim_open_trigger = Animator.StringToHash("Open");
        anim_empty_trigger = Animator.StringToHash("Empty");
    }

    private void Start()
    {
        ChestStorage = new Chest();
        var data = GameDatabase.Instance.Chests[chestId];
        var saveData = SavesManager.Instance.Chests.Find(x => x.chestId == chestId);
        if (saveData != null)
        {
            IsLocked = saveData.isLocked;
            IsEmpty = saveData.isEmpty;
        }
        else
        {
            IsLocked = true;
            IsEmpty = false;
        }
        if (IsEmpty)
        {
            animator.SetTrigger(anim_empty_trigger);
        }
        else
        {
            foreach (int cardId in data.cardsId)
            {
                ChestStorage.AddCard(GameDatabase.Instance.GetCard(cardId), false);
            }
        }
    }

    public void Interact()
    {
        if (CanBeOpen())
        {
            GameManager.Instance.OpenChest(this);
        }
    }

    public bool CanBeOpen()
    {
        return !IsLocked && !IsEmpty;
    }

    public void Unlock()
    {
        IsLocked = false;
        SavesManager.Instance.UpdateChest(this);
    }

    public void StartOpenAnimation()
    {
        animator.SetTrigger(anim_open_trigger);
    }

    public void ChestWasOpen()
    {
        IsEmpty = true;
        BoardSceneUI.Instance.OpenChest(ChestStorage);
        SavesManager.Instance.UpdateChest(this);
    }

    public void Outline(Color color)
    {
        outline.OutlineColor = color;
    }
}

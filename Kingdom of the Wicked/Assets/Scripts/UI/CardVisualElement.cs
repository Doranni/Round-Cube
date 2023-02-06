using UnityEngine;
using UnityEngine.UIElements;

public class CardVisualElement : VisualElement
{
    public Card CardInfo { get; protected set; }
    public IStorage.StorageNames Storage { get; private set; }

    protected VisualElement cardBackground;
    protected Label cardName, cardDescription, cardStatBonuses;

    protected const string k_cardBackground = "CardBackground";
    protected const string k_cardName = "CardName";
    protected const string k_cardDescription = "CardDescription";
    protected const string k_cardStatBonuses = "CardStatBonuses";

    protected CardVisualElement() 
    {
        hierarchy.Add(GameManager.Instance.CardAsset.CloneTree());
        cardBackground = this.Q(k_cardBackground);
        cardName = this.Q<Label>(k_cardName);
        cardDescription = this.Q<Label>(k_cardDescription);
        cardStatBonuses = this.Q<Label>(k_cardStatBonuses);
    }

    public CardVisualElement(Card card, IStorage.StorageNames Storage) : this()
    {
        CardInfo = card;
        this.Storage = Storage;
        StyleCard();
        RegisterCallback<PointerDownEvent, CardVisualElement>(DragAndDropController.Instance.AddTarget, this);
    }

    protected void StyleCard()
    {
        SetText();
        SetSize();
    }

    protected void SetText()
    {
        cardName.text = CardInfo.CardName;
        cardDescription.text = CardInfo.Description;
        var statBonuses = "";
        foreach (StatBonus bonus in CardInfo.StatBonuses)
        {
            statBonuses += GameDatabase.Instance.StatsDescription[bonus.StatTypeId].name + ": "
                + bonus.Value + "\n";
        }
        cardStatBonuses.text = statBonuses;
    }

    protected virtual void SetSize()
    {
        var size = GameManager.Instance.GetRequiredCardSize(Storage);
        cardBackground.style.width = size.x;
        cardBackground.style.height = size.y;
        style.width = size.x;
        style.height = size.y;
        cardBackground.style.position = Position.Absolute;
        cardBackground.style.left = 0;
        cardBackground.style.right = 0;
        cardBackground.style.top = 0;
        cardBackground.style.bottom = 0;
        switch (Storage)
        {
            case IStorage.StorageNames.inventory:
            case IStorage.StorageNames.storage:
                {
                    style.marginBottom = GameManager.Instance.InventoryCardMargin;
                    style.marginTop = GameManager.Instance.InventoryCardMargin;
                    style.marginLeft = GameManager.Instance.InventoryCardMargin;
                    style.marginRight = GameManager.Instance.InventoryCardMargin;
                    break;
                }
        }
    }
}

public class CardToDragVisualElement : CardVisualElement
{
    public CardToDragVisualElement() : base() 
    {
        Clean();
    }

    public void Init(Card card)
    {
        CardInfo = card;
        StyleCard();
    }

    public void Clean()
    {
        CardInfo = null;
        style.display = DisplayStyle.None;
        transform.position = Vector3.zero;
        style.top = 0;
        style.left = 0;
    }

    protected override void SetSize()
    {
        var size = GameManager.Instance.CardSize_big;
        cardBackground.style.width = size.x;
        cardBackground.style.height = size.y;
        style.width = size.x;
        style.height = size.y;
        style.position = Position.Absolute;
    }
}

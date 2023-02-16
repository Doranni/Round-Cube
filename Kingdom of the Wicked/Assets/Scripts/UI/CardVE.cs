using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CardVE : VisualElement
{
    public Card CardData { get; protected set; }
    public IStorage.StorageNames Storage { get; protected set; }

    protected VisualElement cardBackground;
    protected Label cardName, cardDescription, cardStatBonuses;

    protected const string k_cardBackground = "CardBackground";
    protected const string k_cardName = "CardName";
    protected const string k_cardDescription = "CardDescription";
    protected const string k_cardStatBonuses = "CardStatBonuses";

    protected CardVE() 
    {
        hierarchy.Add(GameManager.Instance.CardAsset.CloneTree());
        cardBackground = this.Q(k_cardBackground);
        cardName = this.Q<Label>(k_cardName);
        cardDescription = this.Q<Label>(k_cardDescription);
        cardStatBonuses = this.Q<Label>(k_cardStatBonuses);
    }

    public CardVE(Card card, IStorage.StorageNames storage) : this()
    {
        CardData = card;
        Storage = storage;
        SetText();
        SetSize();
        RegisterCallback<PointerDownEvent, CardVE>(DragAndDropController.Instance.AddTarget, this);
    }

    protected void SetText()
    {
        cardName.text = CardData.CardName;
        cardDescription.text = CardData.Description;
        if (CardData is IAddStatBonuses)
        {
            var statBonuses = "";
            foreach (StatBonus bonus in ((IAddStatBonuses)CardData).StatBonuses)
            {
                statBonuses += GameDatabase.Instance.StatsDescription[bonus.StatTypeId].name + ": "
                    + bonus.Value + "\n";
            }
            cardStatBonuses.text = statBonuses;
        }
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
            case IStorage.StorageNames.Inventory:
            case IStorage.StorageNames.Storage:
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

public class CardToDragVisualElement : CardVE
{
    public CardToDragVisualElement() : base() 
    {
        Clean();
        SetSize();
    }

    public void Init(Card card)
    {
        CardData = card;
        SetText();
    }

    public void Clean()
    {
        CardData = null;
        style.display = DisplayStyle.None;
        transform.position = Vector3.zero;
        style.top = 0;
        style.left = 0;
    }

    protected override void SetSize()
    {
        var size = GameManager.Instance.CardSize_dragging;
        style.position = Position.Absolute;
        style.width = size.x;
        style.height = size.y;
        cardBackground.style.width = size.x;
        cardBackground.style.height = size.y;
    }
}

public class BattleCardVE : CardVE
{
    public BattleCardsHolderVE CardsHolder { get; private set; }

    const string k_cardSelected_class = "card_selected";

    public BattleCardVE(Card card, BattleCardsHolderVE cardsHolder) : base()
    {
        CardData = card;
        CardsHolder = cardsHolder;
        SetText();
        SetSize();
        if (cardsHolder.CardsOwner == FightingManager.Instance.Player)
        {
            RegisterCallback<PointerDownEvent>(_ => SelectCard());
        }
    }

    protected override void SetSize()
    {
        var size = GameManager.Instance.CardSize_slot;
        style.position = Position.Relative;
        style.width = size.x;
        style.height = size.y;
        cardBackground.style.width = size.x;
        cardBackground.style.height = size.y;
        style.marginTop = 5;
        style.marginBottom = 5;
        style.marginLeft = 5;
        style.marginRight = 5;
    }

    public void SelectCard()
    {
        if (GameManager.Instance.State == GameManager.GameState.fighting 
            && FightingManager.Instance.CurrentTurn == CardsHolder.CardsOwner
)
        {
            Debug.Log($"Card {CardData.CardName} was selected");
            CardsHolder.CardsOwner.Fighting.SelectBattleCard(CardData.InstanceId);
        }
    }

    public void AddSelection()
    {
        cardBackground.AddToClassList(k_cardSelected_class);
        style.height = style.height.value.value * 1.1f;
        style.width = style.width.value.value * 1.1f;
        cardBackground.style.height = style.height;
        cardBackground.style.width = style.width;
    }

    public void RemoveSelection()
    {
        cardBackground.RemoveFromClassList(k_cardSelected_class);
        style.height = style.height.value.value / 1.1f;
        style.width = style.width.value.value / 1.1f;
        cardBackground.style.height = style.height;
        cardBackground.style.width = style.width;
    }
}

public class BattleCardsHolderVE : VisualElement
{
    public new class UxmlFactory : UxmlFactory<BattleCardsHolderVE> { }

    public Character CardsOwner { get; private set; }
    public Dictionary<int, BattleCardVE> Cards { get; private set; }

    public BattleCardsHolderVE()
    {
        Cards = new();
    }

    public void Init(Character character)
    {
        CardsOwner = character;
        CardsOwner.Fighting.SelectedCardChanged += OnSelectedCardChanged;
    }

    public void Update()
    {
        Clear();
        Cards.Clear();
        foreach (Card card in CardsOwner.Fighting.BattleCards)
        {
            var battleCard = new BattleCardVE(card, this);
            Add(battleCard);
            Cards.Add(card.InstanceId, battleCard);
        }
    }

    public void UnInit()
    {
        CardsOwner.Fighting.SelectedCardChanged -= OnSelectedCardChanged;
        CardsOwner = null;
        Clear();
        Cards.Clear();
    }

    private void OnSelectedCardChanged((int unselectedCardId, int selectedCardId) cards)
    {
        if (Cards.ContainsKey(cards.unselectedCardId))
        {
            Cards[cards.unselectedCardId].RemoveSelection();
        }
        if (Cards.ContainsKey(cards.selectedCardId))
        {
            Cards[cards.selectedCardId].AddSelection();
        }
    }
}

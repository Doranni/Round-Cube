using System.Collections.Generic;
using UnityEngine.UIElements;

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
        if (FightingManager.Instance.CurrentTurn == FightingManager.Turn.Player)
        {
            CardsHolder.CardsOwner.Deck.SelectBattleCard(CardData.InstanceId);
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
        CardsOwner.Deck.SelectedCardChanged += OnSelectedCardChanged;
    }

    public void Update()
    {
        Clear();
        Cards.Clear();
        CardsOwner.Deck.UpdateCards();
        foreach (Card card in CardsOwner.Deck.BattleCards)
        {
            var battleCard = new BattleCardVE(card, this);
            Add(battleCard);
            Cards.Add(card.InstanceId, battleCard);
        }
    }

    public void UnInit()
    {
        CardsOwner.Deck.SelectedCardChanged -= OnSelectedCardChanged;
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

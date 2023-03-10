using UnityEngine;
using UnityEngine.UIElements;

public class CardVE : VisualElement
{
    public Card CardData { get; protected set; }
    public IStorage.StorageNames Storage { get; protected set; }

    protected VisualElement cardBackground;
    protected Label cardName, cardDescription;

    protected const string k_cardBackground = "CardBackground";
    protected const string k_cardName = "CardName";
    protected const string k_cardDescription = "Description";

    const string k_cardSelected_class = "card_selected";

    protected CardVE() 
    {
        hierarchy.Add(GameManager.Instance.CardAsset.CloneTree());
        cardBackground = this.Q(k_cardBackground);
        cardName = this.Q<Label>(k_cardName);
        cardDescription = this.Q<Label>(k_cardDescription);
    }

    public CardVE(Card card, IStorage.StorageNames storage) : this()
    {
        CardData = card;
        Storage = storage;
        SetText();
        SetSize();
        CardData.WasSelected += OnWasSelected;
        CardData.WasHided += OnWasHided;
        CardData.WasChanged += SetText;
        if (DragCardManager.Instance != null && CardData.Owner != null &&
            CardData.Owner.CharacterId == GameDatabase.Instance.PlayerHeroId)
        {
            RegisterCallback<PointerDownEvent, CardVE>(DragCardManager.Instance.AddTarget, this);
        }
    }

    protected void SetText()
    {
        cardName.text = CardData.CardName;
        switch (CardData.CardType)
        {
            case Card.CardsType.Weapon:
                {
                    cardDescription.text = EffectsToText() + StatBonusesToText();
                    break;
                }
            case Card.CardsType.Armor:
                {
                    cardDescription.text = "Protection: " + ((ArmorCard)CardData).Protection + "\n" 
                        + StatBonusesToText();
                    break;
                }
            case Card.CardsType.Shield:
                {
                    cardDescription.text = "Charges: " + ((ShieldCard)CardData).BlockChargesLeft + "\n"
                        + "Chanse: " + ((ShieldCard)CardData).BlockChanse + "\n"
                        + StatBonusesToText();
                    break;
                }
            case Card.CardsType.Magic:
                {
                    cardDescription.text = EffectsToText() 
                        + "Charges: " + ((MagicCard)CardData).ChargesLeft + "/" + ((MagicCard)CardData).ChargesMax;
                    break;
                }
            case Card.CardsType.Potion:
                {
                    cardDescription.text = EffectsToText() 
                        + "Charges: " + ((PotionCard)CardData).ChargesLeft + "/" + ((PotionCard)CardData).ChargesMax;
                    break;
                }
        }
    }

    private string StatBonusesToText()
    {
        var statBonuses = "";
        foreach (StatBonus bonus in ((ICardAddStatBonuses)CardData).StatBonuses)
        {
            statBonuses += GameDatabase.Instance.StatsDescription[bonus.StatTypeId].name + ": "
                + bonus.Value + "\n";
        }
        return statBonuses;
    }

    private string EffectsToText()
    {
        var effects = "";
        foreach (Effect effect in ((ICardUsable)CardData).Effects)
        {
            effects += effect.Name + "\n";
        }
        return effects;
    }

    protected virtual void SetSize()
    {
        var size = GameManager.Instance.CardSize_regular;
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
            case IStorage.StorageNames.Reward:
                {
                    style.marginBottom = GameManager.Instance.InventoryCardMargin;
                    style.marginTop = GameManager.Instance.InventoryCardMargin;
                    style.marginLeft = GameManager.Instance.InventoryCardMargin;
                    style.marginRight = GameManager.Instance.InventoryCardMargin;
                    break;
                }
            default:
                {
                    style.position = Position.Absolute;
                    style.left = 0;
                    style.right = 0;
                    style.top = 0;
                    style.bottom = 0;
                    break;
                }
        }
    }

    private void OnWasSelected(bool value)
    {
        if (value)
        {
            AddSelection();
        }
        else
        {
            RemoveSelection();
        }
    }

    private void AddSelection()
    {
        cardBackground.AddToClassList(k_cardSelected_class);
        var size = GameManager.Instance.CardSize_selected;
        style.height = size.y;
        style.width = size.x;
        cardBackground.style.height = size.y;
        cardBackground.style.width = size.x;

        switch (Storage)
        {
            case IStorage.StorageNames.Inventory:
            case IStorage.StorageNames.Reward:
                {
                    break;
                }
            default:
                {
                    var offset = (GameManager.Instance.CardSize_selected - GameManager.Instance.CardSize_regular) / -2;
                    style.left = offset.x;
                    style.top = offset.y;
                    break;
                }
        }
    }

    private void RemoveSelection()
    {
        cardBackground.RemoveFromClassList(k_cardSelected_class);
        var size = GameManager.Instance.CardSize_regular;
        style.height = size.y;
        style.width = size.x;
        cardBackground.style.height = size.y;
        cardBackground.style.width = size.x;

        switch (Storage)
        {
            case IStorage.StorageNames.Inventory:
            case IStorage.StorageNames.Reward:
                {
                    break;
                }
            default:
                {
                    style.left = 0;
                    style.top = 0;
                    break;
                }
        }
    }

    private void OnWasHided(bool value)
    {
        if (value)
        {
            style.display = DisplayStyle.None;
        }
        else
        {
            style.display = DisplayStyle.Flex;
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
        var size = GameManager.Instance.CardSize_regular;
        style.position = Position.Absolute;
        style.width = size.x;
        style.height = size.y;
        cardBackground.style.width = size.x;
        cardBackground.style.height = size.y;
    }
}
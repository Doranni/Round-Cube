using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Card> Cards { get; private set; }

    public event Action OnInventoryChanged;

    private void Awake()
    {
        Cards = new();
    }

    public void AddCard(Card card)
    {
        Cards.Add(card);
        OnInventoryChanged?.Invoke();
    }

    public bool RemoveCard(Card card)
    {
        var cardToRemove = Cards.Find(x => x.Id == card.Id);
        if (cardToRemove == null)
        {
            return false;
        }
        Cards.Remove(cardToRemove);
        OnInventoryChanged?.Invoke();
        return true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : Singleton<ItemsDatabase>
{
    [SerializeField] private CardsDatabaseData data;

    public Dictionary<int, CardData> Cards { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Cards = new(data.cards.Count);
        for (int i = 0; i < data.cards.Count; i++)
        {
            Cards.Add(data.cards[i].id, data.cards[i]);
        }
    }

    public Card GetCard(int id)
    {
        if (Cards.ContainsKey(id))
        {
            return new Card(Cards[id]);
        }
        return null;
    }
}

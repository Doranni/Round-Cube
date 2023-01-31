using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : Singleton<ItemsDatabase>
{
    public Dictionary<int, CardData> Cards { get; private set; }

    public override void Awake()
    {
        base.Awake();
        CardData[] data = Resources.LoadAll<CardData>("Cards");
        Cards = new(data.Length);
        for (int i = 0; i < data.Length; i++)
        {
            Cards.Add(data[i].id, data[i]);
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

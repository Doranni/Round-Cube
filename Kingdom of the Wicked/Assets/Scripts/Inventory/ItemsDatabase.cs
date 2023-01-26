using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsDatabase : MonoBehaviour
{
    [SerializeField] private CardsDatabaseData data;

    public Dictionary<string, Card> Cards { get; private set; }

    private void Awake()
    {
        Cards = new(data.cards.Count);
        for (int i = 0; i < data.cards.Count; i++)
        {
            Cards.Add(data.cards[i].cardName, new Card(data.cards[i]));
        }
    }
}

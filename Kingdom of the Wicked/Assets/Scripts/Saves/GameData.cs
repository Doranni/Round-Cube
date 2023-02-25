using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameData
{
    public int playerNodeIndex;
    public List<CharacterData> characters;
    public List<MapNodeData> mapNodes;

    public GameData(int playerNodeIndex, List<CharacterData> characters, List<MapNodeData> mapNodes)
    {
        this.playerNodeIndex = playerNodeIndex;
        this.characters = characters;
        this.mapNodes = mapNodes;
    }
}

[Serializable]
public class CharacterData
{
    public int id;
    public bool isDead;
    public float health;
    public List<EquippedCard> cards;

    public CharacterData(Character character)
    {
        id = character.Id;
        isDead = character.Stats.ChHealth.IsDead;
        health = character.Stats.ChHealth.CurrentHealth;
        cards = new();
        foreach (KeyValuePair<IStorage.StorageNames, IStorage> storage in character.Equipment.Storages)
        {
            foreach (Card card in storage.Value.Cards)
            {
                cards.Add(new EquippedCard(card.NameId, storage.Key));
            }
        }
    }

    public void Update(Character character)
    {
        isDead = character.Stats.ChHealth.IsDead;
        health = character.Stats.ChHealth.CurrentHealth;
        cards.Clear();
        foreach (KeyValuePair<IStorage.StorageNames, IStorage> storage in character.Equipment.Storages)
        {
            foreach (Card card in storage.Value.Cards)
            {
                cards.Add(new EquippedCard(card.NameId, storage.Key));
            }
        }
    }
}

[Serializable]
public class EquippedCard
{
    public int id;
    public IStorage.StorageNames storage;

    public EquippedCard(int id, IStorage.StorageNames storage)
    {
        this.id = id;
        this.storage = storage;
    }
}

[Serializable]
public class MapNodeData
{
    public int index;
    public bool isVisited;

    public MapNodeData(int index, bool isVisited)
    {
        this.index = index;
        this.isVisited = isVisited;
    }
}


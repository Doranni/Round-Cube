using System;
using System.Collections.Generic;

[Serializable]
public class GameData
{
    public int playerCurrentNodeIndex;
    public int playerPrevNodeIndex;
    public List<CharacterData> characters;
    public List<ChestData> chests;
    public List<MapNodeData> mapNodes;

    public GameData(int playerCurrentNodeIndex, int playerPrevNodeIndex, List<CharacterData> characters, List<ChestData> chests,
        List<MapNodeData> mapNodes)
    {
        this.playerCurrentNodeIndex = playerCurrentNodeIndex;
        this.playerPrevNodeIndex = playerPrevNodeIndex;
        this.characters = characters;
        this.chests = chests;
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
        id = character.CharacterId;
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
    public int nodeId;

    public MapNodeData(int nodeId)
    {
        this.nodeId = nodeId;
    }
}

[Serializable]
public class ChestData
{
    public int chestId;
    public bool isLocked;
    public bool isEmpty;

    public ChestData(int chestId, bool isLocked, bool isEmpty)
    {
        this.chestId = chestId;
        this.isLocked = isLocked;
        this.isEmpty = isEmpty;
    }
}


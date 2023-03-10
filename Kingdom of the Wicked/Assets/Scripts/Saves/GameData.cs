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
                cards.Add(new EquippedCard(card));
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
                cards.Add(new EquippedCard(card));
            }
        }
    }
}

[Serializable]
public class EquippedCard
{
    public int id;
    public IStorage.StorageNames storage;
    public int charges;
    public int armor_protection;

    public EquippedCard(Card card)
    {
        id = card.NameId;
        storage = card.Storage;
        switch (card.CardType)
        {
            case Card.CardsType.Armor:
                {
                    armor_protection = ((ArmorCard)card).Protection;
                    break;
                }
            case Card.CardsType.Shield:
                {
                    charges = ((ShieldCard)card).BlockChargesLeft;
                    break;
                }
            case Card.CardsType.Magic:
            case Card.CardsType.Potion:
                {
                    charges = ((ICardBreakable)card).ChargesLeft;
                    break;
                }
        }
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


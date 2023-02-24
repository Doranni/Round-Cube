using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
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

[System.Serializable]
public class CharacterData
{
    public int id;
    public float health;

    public CharacterData(int id, float health)
    {
        this.id = id;
        this.health = health;
    }
}

[System.Serializable]
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


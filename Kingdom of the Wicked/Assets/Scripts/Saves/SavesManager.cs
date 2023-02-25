using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavesManager : Singleton<SavesManager>
{
    private string dataFileName = "data", fullPath;

    public int PlayerNodeIndex { get; private set; }
    public List<CharacterData> Characters { get; private set; }
    public List<MapNodeData> MapNodes { get; private set; }
    public int EnemyForFightId { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        fullPath = Path.Combine(Application.persistentDataPath, dataFileName);
        PlayerNodeIndex = 0;
        Characters = new();
        MapNodes = new();
        Debug.Log(Application.persistentDataPath);
        LoadGame();
    }

    public void UpdatePlayerPos(int playerNodeIndex)
    {
        PlayerNodeIndex = playerNodeIndex;
    }

    public void UpdateCharacter(Character character)
    {
        var characterData = Characters.Find(x => x.id == character.Id);
        if (characterData == null)
        {
            characterData = new CharacterData(character);
            Characters.Add(characterData);
        }
        else
        {
            characterData.Update(character);
        }
    }

    public void UpdateMapNode(int index, bool isVisited)
    {
        var mapNode = MapNodes.Find(x => x.index == index);
        if (mapNode == null)
        {
            mapNode = new MapNodeData(index, isVisited);
            MapNodes.Add(mapNode);
        }
        else
        {
            mapNode.isVisited = isVisited;
        }
    }

    public void SetEnemieForFight(int id)
    {
        EnemyForFightId = id;
    }

    public void SaveGame()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        var data = new GameData(PlayerNodeIndex, Characters, MapNodes);
        string dataToStore = JsonUtility.ToJson(data, true);
        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            using (StreamWriter writer = new StreamWriter(stream))
            {
                writer.Write(dataToStore);
            }
        }
    }

    public void LoadGame()
    {
        GameData data = null;
        if (File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                data = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch (Exception e)
            {
                Debug.LogError("Error occured when trying to load file at path: "
                    + fullPath + e);
            }
        }
        if (data != null)
        {
            PlayerNodeIndex = data.playerNodeIndex;
            Characters = data.characters;
            MapNodes = data.mapNodes;
        }
    }

    public void NewGame()
    {

    }

    private void OnDestroy()
    {
        SaveGame();
    }
}

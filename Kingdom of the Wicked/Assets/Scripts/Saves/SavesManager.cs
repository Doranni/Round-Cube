using System.IO;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SavesManager : Singleton<SavesManager>
{
    private string dataFileName = "data", fullPath;

    public int PlayerNodeIndex { get; private set; }
    public List<CharacterData> Characters { get; private set; }
    public List<ChestData> Chests { get; private set; }
    public List<MapNodeData> MapNodes { get; private set; }
    public int EnemyForFightId { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        fullPath = Path.Combine(Application.persistentDataPath, dataFileName);
        Characters = new();
        Chests = new();
        MapNodes = new();
        Debug.Log(Application.persistentDataPath);
    }

    public void UpdatePlayerPos(int playerNodeIndex)
    {
        PlayerNodeIndex = playerNodeIndex;
    }

    public void UpdateCharacter(Character character)
    {
        var characterData = Characters.Find(x => x.id == character.CharacterId);
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

    public void UpdateChest(ChestController chest)
    {
        var chestData = Chests.Find(x => x.chestId == chest.ChestId);
        if (chestData == null)
        {
            chestData = new ChestData(chest.ChestId, chest.IsLocked, chest.IsEmpty);
            Chests.Add(chestData);
        }
        else
        {
            chestData.isLocked = chest.IsLocked;
            chestData.isEmpty = chest.IsEmpty;
        }
    }

    public void UpdateMapNode(int nodeId)
    {
        var mapNode = MapNodes.Find(x => x.nodeId == nodeId);
        if (mapNode == null)
        {
            mapNode = new MapNodeData(nodeId);
            MapNodes.Add(mapNode);
        }
        else
        {
            
        }
    }

    public void SetEnemieForFight(int id)
    {
        EnemyForFightId = id;
    }

    public void SaveGame()
    {
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
        var data = new GameData(PlayerNodeIndex, Characters, Chests, MapNodes);
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
            Chests = data.chests;
            MapNodes = data.mapNodes;
        }
    }

    public void NewGame()
    {
        PlayerNodeIndex = 1;
        Characters.Clear();
        Chests.Clear();
        MapNodes.Clear();
    }

    private void OnDestroy()
    {
        SaveGame();
    }
}

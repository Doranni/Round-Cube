using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Character ", menuName = "Characters/Character")]
public class CharacterSO : ScriptableObject
{
    public int id;
    public string characterName, characterDescription;
    public int baseHealthValue, armorValue, damageValue;
    public EquippedCardsSO[] cards;
}

[Serializable]
public class EquippedCardsSO 
{
    public int cardId;
    public IStorage.StorageNames storage;
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Character ", menuName = "Characters/Character")]
public class CharacterSO : ScriptableObject
{
    [SerializeField] private int id;
    [SerializeField] private string characterName, characterDescription;
    [SerializeField] private int baseHealthValue, armorValue, damageValue;
    [SerializeField] private EquippedCardsSO[] cards;
    [SerializeField] private Sprite boardSprite, fightSprite;

    public int Id => id;
    public string CharacterName => characterName;
    public string CharacterDescription => characterDescription;
    public int BaseHealthValue => baseHealthValue;
    public int ArmorValue => armorValue;
    public int DamageValue => damageValue;
    public EquippedCardsSO[] Cards => cards;
    public Sprite BoardSprite => boardSprite;
    public Sprite FightSprite => fightSprite;
}

[Serializable]
public class EquippedCardsSO 
{
    [SerializeField] private int cardId;
    [SerializeField] private IStorage.StorageNames storage;

    public int CardId => cardId;
    public IStorage.StorageNames Storage => storage;
}

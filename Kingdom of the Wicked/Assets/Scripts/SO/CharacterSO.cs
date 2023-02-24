using UnityEngine;

[CreateAssetMenu(fileName = "Character ", menuName = "Characters/Character")]
public class CharacterSO : ScriptableObject
{
    public int id;
    public string characterName, characterDescription;
    public int baseHealthValue, armorValue, damageValue;
}

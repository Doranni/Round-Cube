using UnityEngine;

[CreateAssetMenu(fileName = "stat", menuName = "Stats")]
public class StatData : ScriptableObject
{
    public StatType type;
    public string statName;
    public string description;
    public int baseValue;
}

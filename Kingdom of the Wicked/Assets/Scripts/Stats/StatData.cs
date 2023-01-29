using UnityEngine;

[CreateAssetMenu(fileName = "stat", menuName = "Stats/Stat")]
public class StatData : ScriptableObject
{
    public Stat.StatId Id;
    public string statName;
    public string description;
}

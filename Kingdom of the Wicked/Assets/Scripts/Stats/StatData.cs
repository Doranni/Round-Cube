using UnityEngine;

[CreateAssetMenu(fileName = "stat", menuName = "ScriptableObjects/Stat")]
public class StatData : ScriptableObject
{
    public Stat.StatId Id;
    public string statName;
    public string description;
}

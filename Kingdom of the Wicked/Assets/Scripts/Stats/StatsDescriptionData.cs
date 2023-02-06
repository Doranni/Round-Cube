using UnityEngine;

[CreateAssetMenu(fileName = "Stats Description", menuName = "Data/Stats Description")]
public class StatsDescriptionData : ScriptableObject
{
    public string healthStatName, healthStatDescription,
        armorStatName, armorStatDescription,
        damageStatName, damageStatDescription;
}

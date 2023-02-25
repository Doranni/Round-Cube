using UnityEngine;

[CreateAssetMenu(fileName = "Stats Description", menuName = "Characters/Stats Description")]
public class StatsDescriptionSO : ScriptableObject
{
    public string healthStatName, healthStatDescription,
        armorStatName, armorStatDescription,
        damageStatName, damageStatDescription;
}

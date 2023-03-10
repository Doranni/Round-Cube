using UnityEngine;

[CreateAssetMenu(fileName = "Stats Description", menuName = "Characters/Stats Description")]
public class StatsDescriptionSO : ScriptableObject
{
    [SerializeField] private string healthStatName, healthStatDescription;

    public string HealthStatName => healthStatName;
    public string HealthStatDescription => healthStatDescription;
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DebugUI : MonoBehaviour
{
    [SerializeField] private StatsManager plStats;

    private VisualElement debugScreen;
    private Label plStatsLbl;

    const string k_debugScreen = "Debug";
    const string k_plStatsLbl = "lbl_stats";

    const string str_stats = "Player Stats:";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        debugScreen = rootElement.Q(k_debugScreen);
        plStatsLbl = rootElement.Q<Label>(k_plStatsLbl);
    }

    void Update()
    {
        var res = str_stats;
        foreach (KeyValuePair<StatType, Stat> stat in plStats.Stats)
        {
            res += "\n" + stat.Value.Name + ": " + stat.Value.TotalValue;
        }
        plStatsLbl.text = res;
    }
}

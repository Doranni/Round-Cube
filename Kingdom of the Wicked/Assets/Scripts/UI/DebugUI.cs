using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DebugUI : MonoBehaviour
{
    [SerializeField] private Character player;

    private VisualElement debugScreen;
    private Label plStatsLbl;

    const string k_debugScreen = "Debug";
    const string k_plStatsLbl = "lbl_stats";

    const string str_stats = "Player Stats:";
    const string str_effects = "Player Effects:";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        debugScreen = rootElement.Q(k_debugScreen);
        plStatsLbl = rootElement.Q<Label>(k_plStatsLbl);
    }

    void Update()
    {
        var res = str_stats;
        foreach (KeyValuePair<Stat.StatId, Stat> stat in player.Stats.ChStats)
        {
            res += "\n" + GameDatabase.Instance.StatsDescription[stat.Key].name + ": " 
                + stat.Value.BaseValue + " => " + stat.Value.TotalValue;
        }
        res += "\n" + str_effects;
        foreach (Effect effect in player.Stats.Effects)
        {
            res += "\n" + effect.EffectType + ": " + effect.Value + " for " + effect.Duration + " moves.";
        }
        plStatsLbl.text = res;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class DebugUI : MonoBehaviour
{
    [SerializeField] private Character player;
    [SerializeField] private Character enemy;

    private Label plStatsLbl, enemyStatsLbl;

    const string k_plStatsLbl = "Lbl_PlStats";
    const string k_enemyStatsLbl = "Lbl_EnemyStats";

    const string str_player = "Player:";
    const string str_enemy = "Enemy:";
    const string str_stats = "Stats:";
    const string str_effects = "Effects:";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        plStatsLbl = rootElement.Q<Label>(k_plStatsLbl);
        enemyStatsLbl = rootElement.Q<Label>(k_enemyStatsLbl);
    }

    void Update()
    {
        var plRes = str_player + "\n" + str_stats;
        foreach (KeyValuePair<Stat.StatId, Stat> stat in player.Stats.ChStats)
        {
            plRes += "\n" + GameDatabase.Instance.StatsDescription[stat.Key].name + ": " 
                + stat.Value.BaseValue + " => " + stat.Value.TotalValue;
        }
        plRes += "\n" + str_effects;
        foreach (Effect effect in player.Stats.Effects)
        {
            plRes += "\n" + effect.EffectType + ": " + effect.Value + " for " + effect.Duration + " moves.";
        }
        plStatsLbl.text = plRes;

        if (enemy != null)
        {
            var enemyRes = str_enemy + "\n" + str_stats;
            foreach (KeyValuePair<Stat.StatId, Stat> stat in enemy.Stats.ChStats)
            {
                enemyRes += "\n" + GameDatabase.Instance.StatsDescription[stat.Key].name + ": "
                    + stat.Value.BaseValue + " => " + stat.Value.TotalValue;
            }
            enemyRes += "\n" + str_effects;
            foreach (Effect effect in enemy.Stats.Effects)
            {
                enemyRes += "\n" + effect.EffectType + ": " + effect.Value + " for " + effect.Duration + " moves.";
            }
            enemyStatsLbl.text = enemyRes;
        }
    }
}

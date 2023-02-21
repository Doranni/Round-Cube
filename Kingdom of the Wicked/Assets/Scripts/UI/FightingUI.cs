using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class FightingUI : MonoBehaviour
{
    [SerializeField] private Character player, enemy;

    private HealthBarVE playerHealthBar, enemyHealthBar;
    private Label enemyIconLabel;
    private BattleCardsHolderVE playerCards, enemyCards;

    const string k_playerHealthBar = "PlayerHP";
    const string k_enemyHealthBar = "EnemieHP";
    const string k_enemyIconLabel = "Lbl_EnemieIcon";
    const string k_playerCards = "PlayerCards";
    const string k_enemieCards = "EnemieCards";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        playerHealthBar = rootElement.Q<HealthBarVE>(k_playerHealthBar);
        enemyHealthBar = rootElement.Q<HealthBarVE>(k_enemyHealthBar);
        enemyIconLabel = rootElement.Q<Label>(k_enemyIconLabel);
        playerCards = rootElement.Q<BattleCardsHolderVE>(k_playerCards);
        enemyCards = rootElement.Q<BattleCardsHolderVE>(k_enemieCards);
    }

    private void Start()
    {
        enemyIconLabel.text = enemy.name;
        playerHealthBar.Init(player.Stats.ChHealth);
        enemyHealthBar.Init(enemy.Stats.ChHealth);
        playerCards.Init(player);
        playerCards.Update();
        enemyCards.Init(enemy);
        enemyCards.Update();
    }
}

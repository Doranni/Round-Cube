using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class FightingSceneUI : Singleton<FightingSceneUI>
{
    [SerializeField] private Character player, enemy;

    private VisualElement victoryScreen, defeatScreen;
    private Button victoryConfirmButton, defeatConfirmButton;
    private RewardVE victoryReward;

    private Label fightStartsLabel, timerLabel, playerTurnLabel, enemyTurnLabel;

    private VisualElement playerScreen, enemyScreen;
    private HealthBarVE playerHealthBar, enemyHealthBar;
    private Label enemyIconLabel;
    private BattleCardsHolderVE playerCards, enemyCards;

    const string k_victoryScreen = "Victory";
    const string k_defeatScreen = "Defeat";
    const string k_victoryConfirmButton = "victoryConfirmButton";
    const string k_defeatConfirmButton = "defeatConfirmButton";
    const string k_victoryReward = "VictoryRewardContent";

    const string k_fightStartsLabel = "labelFightStarts";
    const string k_timerLabel = "labelTimer";
    const string k_playerTurnLabel = "labelPlayerTurn";
    const string k_enemyTurnLabel = "labelEnemyTurn";

    const string k_playerScreen = "Player";
    const string k_enemyScreen = "Enemie";
    const string k_playerHealthBar = "PlayerHP";
    const string k_enemyHealthBar = "EnemieHP";
    const string k_enemyIconLabel = "Lbl_EnemieIcon";
    const string k_playerCards = "PlayerCards";
    const string k_enemieCards = "EnemieCards";

    const string anim_fightStartsLabel_hiden = "fightStarts_label_hiden";
    const string anim_turnLabel_hiden = "turn_label_hiden";

    private int frames;

    protected override void Awake()
    {
        base.Awake();

        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        victoryScreen = rootElement.Q(k_victoryScreen);
        defeatScreen = rootElement.Q(k_defeatScreen);
        victoryConfirmButton = rootElement.Q<Button>(k_victoryConfirmButton);
        defeatConfirmButton = rootElement.Q<Button>(k_defeatConfirmButton);
        victoryReward = rootElement.Q<RewardVE>(k_victoryReward);

        fightStartsLabel = rootElement.Q<Label>(k_fightStartsLabel);
        timerLabel = rootElement.Q<Label>(k_timerLabel);
        playerTurnLabel = rootElement.Q<Label>(k_playerTurnLabel);
        enemyTurnLabel = rootElement.Q<Label>(k_enemyTurnLabel);

        playerScreen = rootElement.Q(k_playerScreen);
        enemyScreen = rootElement.Q(k_enemyScreen);
        playerHealthBar = rootElement.Q<HealthBarVE>(k_playerHealthBar);
        enemyHealthBar = rootElement.Q<HealthBarVE>(k_enemyHealthBar);
        enemyIconLabel = rootElement.Q<Label>(k_enemyIconLabel);
        playerCards = rootElement.Q<BattleCardsHolderVE>(k_playerCards);
        enemyCards = rootElement.Q<BattleCardsHolderVE>(k_enemieCards);

        victoryScreen.style.display = DisplayStyle.None;
        defeatScreen.style.display = DisplayStyle.None;
        timerLabel.style.display = DisplayStyle.None;
        playerScreen.style.display = DisplayStyle.None;
        enemyScreen.style.display = DisplayStyle.None;

        rootElement.RegisterCallback<GeometryChangedEvent>(OnGeometryChangedEvent);
        frames = 0;
    }

    private void Update()
    {
        frames++;
    }

    private void OnGeometryChangedEvent(GeometryChangedEvent evt)
    {
        Debug.Log("OnGeometryChangedEvent, frames = " + frames);
    }

    private void Start()
    {
        enemyIconLabel.text = enemy.CharacterName;
        playerHealthBar.Init(player.Stats.ChHealth);
        enemyHealthBar.Init(enemy.Stats.ChHealth);
        playerCards.Init(player);
        playerCards.Update();
        enemyCards.Init(enemy);
        enemyCards.Update();

        victoryConfirmButton.RegisterCallback<ClickEvent>(_ => OnVictoryButtonClick());
        defeatConfirmButton.RegisterCallback<ClickEvent>(_ => GameManager.Instance.EndFight());
    }

    private void OnVictoryButtonClick()
    {
        for (int i = 0; i < victoryReward.Storage.Cards.Count; i++)
        {
            player.Equipment.AddCard(victoryReward.Storage.Cards[i], IStorage.StorageNames.Inventory, false, true);
        }
        victoryReward.Reset();
        GameManager.Instance.EndFight();
    }

    public void StartFight()
    {
        StartCoroutine(ShowLabel(fightStartsLabel, anim_fightStartsLabel_hiden, 2));
    }

    public void StartPlayerTurn()
    {
        StartCoroutine(ShowLabel(playerTurnLabel, anim_turnLabel_hiden, 1));
    }

    public void StartEnemiesTurn()
    {
        StartCoroutine(ShowLabel(enemyTurnLabel, anim_turnLabel_hiden, 1));
    }

    private IEnumerator ShowLabel(Label label, string animClass, float time)
    {
        label.ToggleInClassList(animClass);
        yield return new WaitForSeconds(time);
        label.ToggleInClassList(animClass);
    }

    public void Victory(Reward reward)
    {
        victoryReward.Init(reward);
        victoryScreen.style.display = DisplayStyle.Flex;
    }

    public void Defeat()
    {
        defeatScreen.style.display = DisplayStyle.Flex;
    }

    public void UpdateTimer()
    {
        timerLabel.text = FightingManager.Instance.TimeLeft.ToString("00.00");
    }

    public void SetPlayerScreenVisible(bool value)
    {
        if (value)
        {
            playerScreen.style.display = DisplayStyle.Flex;
        }
        else
        {
            playerScreen.style.display = DisplayStyle.None;
        }
    }

    public void SetEnemyScreenVisible(bool value)
    {
        if (value)
        {
            enemyScreen.style.display = DisplayStyle.Flex;
        }
        else
        {
            enemyScreen.style.display = DisplayStyle.None;
        }
    }

    public void SetTimerVisible(bool value)
    {
        if (value)
        {
            timerLabel.style.display = DisplayStyle.Flex;
        }
        else
        {
            timerLabel.style.display = DisplayStyle.None;
        }
    }
}

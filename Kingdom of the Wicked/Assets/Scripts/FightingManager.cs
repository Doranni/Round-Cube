using System.Collections;
using UnityEngine;

public class FightingManager : Singleton<FightingManager>
{
    public enum Turn
    {
        None,
        Player,
        Enemy
    }

    [SerializeField] private int maxTimerValue;
    [SerializeField] private PlayerController player;
    [SerializeField] private EnemyController enemy;

    public PlayerController Player => player;
    public EnemyController Enemy => enemy;
    public Turn CurrentTurn { get; private set; }
    public int TimeLeft { get; private set; }
    private Reward reward;

    private void Start()
    {
        CurrentTurn = Turn.None;
        Enemy.InitCharacter(SavesManager.Instance.EnemyForFightId);
        StartCoroutine(StartFight());
        reward = new();
    }

    public IEnumerator StartFight()
    {
        yield return new WaitForSeconds(0.3f);
        FightingSceneUI.Instance.StartFight();
        yield return new WaitForSeconds(2);
        FightingSceneUI.Instance.SetPlayerScreenVisible(true);
        FightingSceneUI.Instance.SetEnemyScreenVisible(true);
        TimeLeft = maxTimerValue;
        FightingSceneUI.Instance.UpdateTimer();
        FightingSceneUI.Instance.SetTimerVisible(true);
        StartCoroutine(StartTurn(Turn.Player));
    }

    public void EndFight()
    {
        CurrentTurn = Turn.None;
        FightingSceneUI.Instance.SetPlayerScreenVisible(false);
        FightingSceneUI.Instance.SetEnemyScreenVisible(false);
        FightingSceneUI.Instance.SetTimerVisible(false);
    }

    public IEnumerator StartTurn(Turn turn)
    {
        TimeLeft = maxTimerValue;
        FightingSceneUI.Instance.UpdateTimer();
        yield return new WaitForSeconds(0.5f);
        CurrentTurn = turn;
        if (CurrentTurn == Turn.Player)
        {
            FightingSceneUI.Instance.StartPlayerTurn();
        }
        else
        {
            FightingSceneUI.Instance.StartEnemiesTurn();
        }
        yield return new WaitForSeconds(0.7f);
        InvokeRepeating(nameof(DecreaseTimer), 1f, 1);
        if (CurrentTurn == Turn.Enemy)
        {
            StartCoroutine(EnemyFightRoutine());
        }
    }

    private void EndTurn()
    {
        Player.Stats.ExecuteEffects();
        Enemy.Stats.ExecuteEffects();
        if (CurrentTurn == Turn.Player)
        {
            Player.Deck.UnselectCards();
        }
        else
        {
            Enemy.Deck.UnselectCards();
        }
        CancelInvoke(nameof(DecreaseTimer));
        if (Player.Stats.ChHealth.IsDead)
        {
            EndFight();
            FightingSceneUI.Instance.Defeat();
            return;
        }
        if (Enemy.Stats.ChHealth.IsDead)
        {
            foreach(IStorage storage in Enemy.Equipment.Storages.Values)
            {
                for(int i = 0; i < storage.Cards.Count; i++)
                {
                    reward.AddCard(storage.Cards[i]);
                    storage.RemoveCard(storage.Cards[i], true);
                    --i;
                }
            }
            EndFight();
            FightingSceneUI.Instance.Victory(reward);
            return;
        }
        if (CurrentTurn == Turn.Player)
        {
            StartCoroutine(StartTurn(Turn.Enemy));
        }
        else
        {
            StartCoroutine(StartTurn(Turn.Player));
        }
        CurrentTurn = Turn.None;
    }

    private IEnumerator EnemyFightRoutine()
    {
        yield return new WaitForSeconds(0.6f);
        (Card card, bool useOnYourself) = Enemy.ChooseCard();
        Enemy.Deck.SelectBattleCard(card.InstanceId);
        yield return new WaitForSeconds(0.6f);
        if (useOnYourself)
        {
            ((IUsable)card).Use(Enemy);
        }
        else
        {
            ((IUsable)card).Use(Player);
        }
        EndTurn();
    }

    public bool TrySetTarget(Character target)
    {
        if (IsTarget(target))
        {
            ((IUsable)Player.Deck.SelectedBattleCard).Use(Enemy);
            EndTurn();
            return true;
        }
        return false;
    }

    public bool IsTarget(Character target)
    {
        if (CurrentTurn == Turn.Player && Player.Deck.SelectedBattleCard != null)
        {
            return true;
        }
        return false;
    }

    private void DecreaseTimer()
    {
        TimeLeft--;
        FightingSceneUI.Instance.UpdateTimer();
        if (TimeLeft == 0)
        {
            EndTurn();
        }
    }
}

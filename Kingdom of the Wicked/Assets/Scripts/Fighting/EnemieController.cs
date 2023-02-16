using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemieController : MonoBehaviour
{
    [SerializeField] private Character enemie;
    [SerializeField] private List<int> cards;

    void Start()
    {
        enemie.Stats.ChHealth.Died += Death;

        foreach(int cardId in cards)
        {
            var card = GameDatabase.Instance.GetCard(cardId);
            enemie.Equipment.EquipCard(card);
        }
    }

    public void StartFight()
    {
        FightingManager.Instance.StartTurn += OnEnemiesTurn;
    }

    public void EndFight()
    {
        FightingManager.Instance.StartTurn -= OnEnemiesTurn;
    }

    private void OnEnemiesTurn(Character character)
    {
        if (character == enemie)
        {
            StartCoroutine(FightRoutine());
        }
    }

    private IEnumerator FightRoutine()
    {
        yield return new WaitForSeconds(0.5f);
        enemie.Fighting.SelectBattleCard(enemie.Fighting.BattleCards[0].InstanceId);
        yield return new WaitForSeconds(0.5f);
        enemie.Fighting.UseBattleCard(FightingManager.Instance.Player);
    }

    private void Death()
    {
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : Singleton<DiceRoller>
{
    [SerializeField] private uint diceRange = 6;
    [SerializeField] private float timeDelay = 0.1f;
    [SerializeField] private PlayerMovement plMovement;

    private Coroutine rollTheDiceRoutine;

    private int diceResult = 0;
    public int DiceResult => diceResult;

    public event System.Action<int> OnDiceResChanged, OnDiceRolled;

    private void Start()
    {
        plMovement.OnMoveFinished += DecreaseDiceRes;
    }

    private void DecreaseDiceRes()
    {
        diceResult--;
        OnDiceResChanged?.Invoke(diceResult);
    }

    public void RollTheDice()
    {
        rollTheDiceRoutine = StartCoroutine(RollTheDiceRoutine());
    }

    private IEnumerator RollTheDiceRoutine()
    {
        diceResult = Random.Range(1, (int)diceRange + 1);
        yield return new WaitForSeconds(timeDelay);
        OnDiceRolled?.Invoke(diceResult);
    }
}

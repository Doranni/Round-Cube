using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : Singleton<DiceRoller>
{
    [SerializeField] private uint diceRange = 6;
    [SerializeField] private PlayerMovement plMovement;

    private int diceResult = 0;
    public int DiceResult => diceResult;

    public event System.Action<int> OnDiceResChanged;

    private void Start()
    {
        plMovement.OnMoveFinished += IncreaseDiceRes;
    }

    private void IncreaseDiceRes()
    {
        diceResult--;
        OnDiceResChanged?.Invoke(diceResult);
    }

    public void RollTheDice()
    {
        diceResult = Random.Range(1, (int)diceRange + 1);
        OnDiceResChanged?.Invoke(diceResult);
    }
}

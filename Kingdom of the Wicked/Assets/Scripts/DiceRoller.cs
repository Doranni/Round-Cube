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
        plMovement.OnStepFinished += DecreaseDiceRes;
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
        var availableDiceValues = GetsAvailableDiceValues();
        if (availableDiceValues.Count > 0)
        {
            diceResult = availableDiceValues[Random.Range(0, availableDiceValues.Count)];
        }
        else
        {
            diceResult = availableDiceValues[Random.Range(1, (int)diceRange + 1)];
        }
        yield return new WaitForSeconds(timeDelay);
        OnDiceRolled?.Invoke(diceResult);
    }

    private List<int> GetsAvailableDiceValues()
    {
        List<int> diceValues = new();

        List<(int distance, int nodeIndex)>nodes = new()
        {
            (0, plMovement.NodeIndex)
        };

        for (int i = 1; i <= diceRange; i++)
        {
            var prevNodes = nodes.FindAll(x => x.distance == i - 1);
            foreach ((int distance, int nodeIndex) in prevNodes)
            {
                var availableLinks = Map.Instance.GetAvailableLinks(nodeIndex);
                foreach (NodeLink link in availableLinks)
                {
                    nodes.Add((i, link.NodeTo.Index));
                }
            }
        }

        for (int i = 1; i <= diceRange; i++)
        {
            var availableNodes = nodes.FindAll(x => x.distance == i);
            foreach ((int distance, int nodeIndex) in availableNodes)
            {
                if (!Map.Instance.MapNodes[nodeIndex].NEvent.IsVisited)
                {
                    diceValues.Add(i);
                    break;
                }
            }
        }
        string diceValuesRes = "Dice values: ";
        foreach (int value in diceValues)
        {
            diceValuesRes += value + " "; ;
        }
        Debug.Log(diceValuesRes);

        return diceValues;
    }
}

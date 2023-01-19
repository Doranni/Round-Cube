using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainSceneUI : MonoBehaviour
{
    private VisualElement diceScreen;
    private Button rollDiceButton;

    const string k_diceScreen = "Dice";
    const string k_diceButton = "Button_RollTheDice";

    const string str_rollTheDice = "Roll the\nDice";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        diceScreen = rootElement.Q(k_diceScreen);
        rollDiceButton = rootElement.Q<Button>(k_diceButton);
    }

    void Start()
    {
        DiceRoller.Instance.OnDiceRolled += DisplayDiceRes;
        DiceRoller.Instance.OnDiceResChanged += DisplayDiceRes;

        rollDiceButton.RegisterCallback<ClickEvent>((_) => DiceRoller.Instance.RollTheDice());

        DisplayDiceRes(DiceRoller.Instance.DiceResult);
    }

    private void DisplayDiceRes(int value)
    {
        if (value == 0)
        {
            rollDiceButton.text = str_rollTheDice;
            rollDiceButton.SetEnabled(true);
        }
        else
        {
            rollDiceButton.text = value.ToString();
            rollDiceButton.SetEnabled(false);
        }
    }

    private void OnDestroy()
    {
        DiceRoller.Instance.OnDiceRolled -= DisplayDiceRes;
        DiceRoller.Instance.OnDiceResChanged -= DisplayDiceRes;
    }
}

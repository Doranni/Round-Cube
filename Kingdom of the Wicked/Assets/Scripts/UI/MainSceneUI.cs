using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainSceneUI : MonoBehaviour
{
    //Dise
    private VisualElement diceScreen;
    private Button rollDiceButton;

    const string k_diceScreen = "Dice";
    const string k_diceButton = "Button_RollTheDice";

    const string str_rollTheDice = "Roll the\nDice";

    //Player
    [SerializeField] private CharacterStats plStats;

    private VisualElement playerScreen;
    private Label plHealthLbl;

    const string k_playerScreen = "Player";
    const string k_plHealthLbl = "Lbl_PlHealth";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        //Dise
        diceScreen = rootElement.Q(k_diceScreen);
        rollDiceButton = rootElement.Q<Button>(k_diceButton);

        //Player
        playerScreen = rootElement.Q(k_playerScreen);
        plHealthLbl = rootElement.Q<Label>(k_plHealthLbl);
    }

    void Start()
    {
        //Dise
        DiceRoller.Instance.OnDiceRolled += DisplayDiceRes;
        DiceRoller.Instance.OnDiceResChanged += DisplayDiceRes;

        rollDiceButton.RegisterCallback<ClickEvent>((_) => DiceRoller.Instance.RollTheDice());

        DisplayDiceRes(DiceRoller.Instance.DiceResult);

        //Player
        plStats.ChHealth.OnChangeHealth += DisplayPlHealth;

        DisplayPlHealth((plStats.ChHealth.CurrentHealth, plStats.ChHealth.MaxHealth));
    }

    private void DisplayPlHealth((float currentHealth, float maxHealth) obj)
    {
        plHealthLbl.text = "HP: " + obj.currentHealth + "/" + obj.maxHealth;
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

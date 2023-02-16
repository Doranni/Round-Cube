using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class EnemieUI : Singleton<EnemieUI>
{
    private Character enemie;

    private VisualElement enemieScreen;
    private Label healthLbl, iconLabel;
    private BattleCardsHolderVE enemieCards;

    const string k_enemieScreen = "Enemie";
    const string k_healthLbl = "Lbl_EnemieHealth";
    const string k_iconLabel = "Lbl_EnemieIcon";
    const string k_enemieCards = "EnemieCards";

    public override void Awake()
    {
        base.Awake();

        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;
        enemieScreen = rootElement.Q(k_enemieScreen);
        healthLbl = rootElement.Q<Label>(k_healthLbl);
        iconLabel = rootElement.Q<Label>(k_iconLabel);
        enemieCards = rootElement.Q<BattleCardsHolderVE>(k_enemieCards);

        enemieScreen.style.display = DisplayStyle.None;
    }

    public void StartFight(Character enemie)
    {
        this.enemie = enemie;

        iconLabel.text = enemie.name;
        enemie.Stats.ChHealth.HealthChanged += DisplayHealth;
        DisplayHealth((enemie.Stats.ChHealth.CurrentHealth, enemie.Stats.ChHealth.MaxHealth));
        enemieCards.Init(enemie);
        enemieCards.Update();

        enemieScreen.style.display = DisplayStyle.Flex;
    }

    public void EndFight()
    {
        enemieCards.UnInit();
        enemie.Stats.ChHealth.HealthChanged -= DisplayHealth;
        enemieScreen.style.display = DisplayStyle.None;
    }

    private void DisplayHealth((float currentHealth, float maxHealth) obj)
    {
        healthLbl.text = "HP: " + obj.currentHealth + "/" + obj.maxHealth;
    }
}

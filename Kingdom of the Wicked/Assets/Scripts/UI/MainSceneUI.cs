using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainSceneUI : MonoBehaviour
{
    //Player
    [SerializeField] private Character player;

    private VisualElement playerScreen;
    private Label plHealthLbl;

    const string k_playerScreen = "Player";
    const string k_plHealthLbl = "Lbl_PlHealth";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        //Player
        playerScreen = rootElement.Q(k_playerScreen);
        plHealthLbl = rootElement.Q<Label>(k_plHealthLbl);
    }

    void Start()
    {
        //Player
        player.Stats.ChHealth.HealthChanged += DisplayPlHealth;

        DisplayPlHealth((player.Stats.ChHealth.CurrentHealth, player.Stats.ChHealth.MaxHealth));
    }

    private void DisplayPlHealth((float currentHealth, float maxHealth) obj)
    {
        plHealthLbl.text = "HP: " + obj.currentHealth + "/" + obj.maxHealth;
    }


    private void OnDestroy()
    {

    }
}

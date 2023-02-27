using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]
public class MainMenuUI : MonoBehaviour
{
    private Button newGameButton, loadGameButton, exitButton;

    const string k_newGameButton = "Btn_NewGame";
    const string k_loadGameButton = "Btn_LoadGame";
    const string k_exitButton = "Btn_Exit";

    void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        newGameButton = rootElement.Q<Button>(k_newGameButton);
        loadGameButton = rootElement.Q<Button>(k_loadGameButton);
        exitButton = rootElement.Q<Button>(k_exitButton);
    }

    private void Start()
    {
        newGameButton.RegisterCallback<ClickEvent>(_ => NewGame());
        loadGameButton.RegisterCallback<ClickEvent>(_ => LoadGame());
        exitButton.RegisterCallback<ClickEvent>(_ => Exit());
    }

    private void NewGame()
    {
        SavesManager.Instance.NewGame();
        LoadSceneManager.Instance.LoadScene(LoadSceneManager.Scenes.Board);
    }

    private void LoadGame()
    {
        SavesManager.Instance.LoadGame();
        LoadSceneManager.Instance.LoadScene(LoadSceneManager.Scenes.Board);
    }

    private void Exit()
    {
        Application.Quit();
    }
}

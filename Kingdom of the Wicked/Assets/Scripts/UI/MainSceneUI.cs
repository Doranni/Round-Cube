using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(UIDocument))]

public class MainSceneUI : MonoBehaviour
{
    // Load Scene
    private VisualElement loadSceneScreen;
    private ProgressBar loadSceneProgressBar;

    const string k_loadScene = "LoadScene";
    const string k_loadSceneProgressBar = "ProgressBar_LoadingScene";

    // Menu
    private VisualElement menuScreen;
    private Button continueButton, quitButton;

    const string k_menuScreen = "Menu";
    const string k_continueButton = "ButtonContinue";
    const string k_quitButton = "ButtonQuit";

    private void Awake()
    {
        VisualElement rootElement = GetComponent<UIDocument>().rootVisualElement;

        // Load Scene
        loadSceneScreen = rootElement.Q(k_loadScene);
        loadSceneProgressBar = rootElement.Q<ProgressBar>(k_loadSceneProgressBar);

        LoadSceneManager.Instance.LoadingStarted += StartLoad;
        LoadSceneManager.Instance.LoadingFinished += FinishLoad;
        LoadSceneManager.Instance.Loading += UpdateLoadingProgressBar;

        // Menu
        menuScreen = rootElement.Q(k_menuScreen);
        continueButton = rootElement.Q<Button>(k_continueButton);
        quitButton = rootElement.Q<Button>(k_quitButton);

        menuScreen.style.display = DisplayStyle.None;
        InputManager.Instance.UIEscape_performed += _ => OpenMenuScreen();
        continueButton.RegisterCallback<ClickEvent>(_ => CloseMenuScreen());
        quitButton.RegisterCallback<ClickEvent>(_ => QuitGame());
    }

    // Load Scene
    private void StartLoad()
    {
        loadSceneScreen.style.display = DisplayStyle.Flex;
    }

    private void FinishLoad()
    {
        loadSceneScreen.style.display = DisplayStyle.None;
    }

    private void UpdateLoadingProgressBar(float value)
    {
        loadSceneProgressBar.value = value;
    }

    // Menu
    private void OpenMenuScreen()
    {
        if (GameManager.Instance.GameIsActive)
        {
            menuScreen.style.display = DisplayStyle.Flex;
            GameManager.Instance.GameIsActive = false;
        }
    }

    private void CloseMenuScreen()
    {
        menuScreen.style.display = DisplayStyle.None;
        GameManager.Instance.GameIsActive = true;
    }

    private void QuitGame()
    {
        SavesManager.Instance.SaveGame();
        LoadSceneManager.Instance.LoadScene(LoadSceneManager.Scenes.Menu);
        menuScreen.style.display = DisplayStyle.None;
    }

    
}

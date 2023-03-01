using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneManager : Singleton<LoadSceneManager>
{
    public enum Scenes
    {
        Main,
        Menu,
        Board,
        Fighting
    }

    public enum LoadState
    {
        Loading,
        Menu,
        Board,
        Fighting
    }

    [SerializeField] private Camera mainCamera;

    public LoadState State { get; private set; }

    private AsyncOperation loadingOperation;
    private Scenes currentScene;

    public event Action LoadingStarted, LoadingFinished;
    public event Action<float> Loading;

    protected override void Awake()
    {
        base.Awake();
        currentScene = Scenes.Main;
    }

    private void Start()
    {
        LoadScene(Scenes.Menu);
    }

    public void LoadScene(Scenes scene)
    {
        if (scene == Scenes.Main)
        {
            return;
        }
        State = LoadState.Loading;
        GameManager.Instance.UpdateState();
        if (currentScene != Scenes.Main)
        {
            SceneManager.UnloadSceneAsync((int)currentScene);
        }
        LoadingStarted?.Invoke();
        loadingOperation = SceneManager.LoadSceneAsync((int)scene, LoadSceneMode.Additive);
        currentScene = scene;
        StartCoroutine(LoadingRoutine());
    }

    private IEnumerator LoadingRoutine()
    {
        while (!loadingOperation.isDone)
        {
            Loading?.Invoke(loadingOperation.progress);
            yield return null;
        }
        State = (LoadState)(int)currentScene;
        GameManager.Instance.UpdateState();
        LoadingFinished?.Invoke();
    }
}

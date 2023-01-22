using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public event Action<InputAction.CallbackContext> OnMouseClick_started, OnMouseClick_performed, OnMouseClick_canceled;

    private GameInput gameInput;

    public override void Awake()
    {
        base.Awake();
        gameInput = new GameInput();
    }

    private void Start()
    {
        gameInput.Player.MouseClick.started += MouseClick_started; ;
        gameInput.Player.MouseClick.performed += MouseClick_performed;
        gameInput.Player.MouseClick.canceled += MouseClick_canceled; ;
    }

    private void MouseClick_started(InputAction.CallbackContext obj)
    {
        OnMouseClick_started?.Invoke(obj);
    }

    private void MouseClick_performed(InputAction.CallbackContext obj)
    {
        OnMouseClick_performed?.Invoke(obj);
    }

    private void MouseClick_canceled(InputAction.CallbackContext obj)
    {
        OnMouseClick_canceled?.Invoke(obj);
    }

    private void OnEnable()
    {
        gameInput.Enable();
    }

    private void OnDisable()
    {
        gameInput.Disable();
    }

    private void OnDestroy()
    {
        gameInput.Player.MouseClick.performed -= MouseClick_performed;
    }
}

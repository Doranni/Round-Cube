using System;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public event Action<InputAction.CallbackContext> UIEscape_performed,
        CameraMove_performed, CameraZoom_performed;

    private GameInput gameInput;

    public override void Awake()
    {
        base.Awake();
        gameInput = new GameInput();
    }

    private void Start()
    {
        gameInput.GameUI.Escape.performed += OnUIEscape_performed;
        gameInput.Camera.Move.performed += OnCameraMove_performed;
        gameInput.Camera.Zoom.performed += OnCameraZoom_performed;
    }

    private void OnUIEscape_performed(InputAction.CallbackContext obj)
    {
        UIEscape_performed?.Invoke(obj);
    }

    private void OnCameraZoom_performed(InputAction.CallbackContext obj)
    {
        CameraZoom_performed?.Invoke(obj);
    }

    private void OnCameraMove_performed(InputAction.CallbackContext obj)
    {
        CameraMove_performed?.Invoke(obj);
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
        gameInput.GameUI.Escape.performed -= OnUIEscape_performed;
        gameInput.Camera.Move.performed -= OnCameraMove_performed;
        gameInput.Camera.Zoom.performed -= OnCameraZoom_performed;
    }
}

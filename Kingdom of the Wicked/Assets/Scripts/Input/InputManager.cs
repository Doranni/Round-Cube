using System;
using UnityEngine.InputSystem;

public class InputManager : Singleton<InputManager>
{
    public event Action<InputAction.CallbackContext> OnCameraMove_performed, OnCameraZoom_performed;

    private GameInput gameInput;

    public override void Awake()
    {
        base.Awake();
        gameInput = new GameInput();
    }

    private void Start()
    {
        gameInput.Camera.Move.performed += CameraMove_performed;
        gameInput.Camera.Zoom.performed += CameraZoom_performed;
    }

    private void CameraZoom_performed(InputAction.CallbackContext obj)
    {
        OnCameraZoom_performed?.Invoke(obj);
    }

    private void CameraMove_performed(InputAction.CallbackContext obj)
    {
        OnCameraMove_performed?.Invoke(obj);
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
        gameInput.Camera.Move.performed -= CameraMove_performed;
        gameInput.Camera.Zoom.performed -= CameraZoom_performed;
    }
}

using UnityEngine;
using Cinemachine;

public class BoardCameraController : Singleton<BoardCameraController>
{
    [SerializeField] private CinemachineVirtualCamera followCamera, focusCamera;
    [SerializeField] private float zoomSpeed = 50, zoomStrength = 400;
    [SerializeField] private float followCamDistMin = 10, followCamDistMax = 60;

    private CinemachineFramingTransposer followFrTransposer;

    private float cameraZoomInput;
    private float destDistFollowCam;

    private void Start()
    {
        followFrTransposer = followCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        InputManager.Instance.CameraZoom_performed += CameraZoom_performed;

        followFrTransposer.m_CameraDistance = 20;
        destDistFollowCam = 20;
        UnsetFocusTarget();
    }

    private void CameraZoom_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.State == GameManager.GameState.BoardActive)
        {
            cameraZoomInput = Mathf.Clamp(obj.ReadValue<float>(), -1, 1);
            destDistFollowCam = Mathf.Clamp(destDistFollowCam + cameraZoomInput * zoomStrength * Time.deltaTime,
                followCamDistMin, followCamDistMax);
        }
    }

    private void LateUpdate()
    {
        if (followFrTransposer.m_CameraDistance != destDistFollowCam)
        {
            followFrTransposer.m_CameraDistance = Mathf.MoveTowards(followFrTransposer.m_CameraDistance,
                destDistFollowCam, zoomSpeed * Time.deltaTime);
        }
    }

    public void SetFocusTarget(Transform target)
    {
        focusCamera.Follow = target;
        focusCamera.LookAt = target;
        focusCamera.Priority = 11;
        followCamera.Priority = 10;
    }

    public void UnsetFocusTarget()
    {
        focusCamera.Priority = 10;
        followCamera.Priority = 11;
    }

    private void OnDestroy()
    {
        InputManager.Instance.CameraZoom_performed -= CameraZoom_performed;
    }
}

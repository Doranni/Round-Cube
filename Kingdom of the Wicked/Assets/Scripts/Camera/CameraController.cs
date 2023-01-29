using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera followCamera;
    [SerializeField] private float zoomSpeed = 50, zoomStrength = 400;
    [SerializeField] private float followCamDistMin = 10, followCamDistMax = 60;

    private CinemachineFramingTransposer followFrTransposer;

    private float cameraZoomInput;
    private float destDistFollowCam;

    private void Start()
    {
        followFrTransposer = followCamera.GetCinemachineComponent<CinemachineFramingTransposer>();

        InputManager.Instance.OnCameraZoom_performed += CameraZoom_performed;

        destDistFollowCam = followFrTransposer.m_CameraDistance;
    }

    private void CameraZoom_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.State == GameManager.GameState.active)
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

    private void OnDestroy()
    {
        InputManager.Instance.OnCameraZoom_performed -= CameraZoom_performed;
    }
}

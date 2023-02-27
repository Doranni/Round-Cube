using UnityEngine;
using Cinemachine;

public class BoardCameraController : MonoBehaviour
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

        InputManager.Instance.CameraZoom_performed += CameraZoom_performed;

        followFrTransposer.m_CameraDistance = 20;
        destDistFollowCam = 20;
    }

    private void CameraZoom_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.GameIsActive)
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
        InputManager.Instance.CameraZoom_performed -= CameraZoom_performed;
    }
}

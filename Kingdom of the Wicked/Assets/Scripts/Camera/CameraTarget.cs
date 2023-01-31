using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    [SerializeField] private float speed = 30, radius = 10;

    private Vector2 cameraInput;
    private Vector3 position;

    void Start()
    {
        InputManager.Instance.OnCameraMove_performed += CameraMove_performed;
    }

    private void CameraMove_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        cameraInput = obj.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        if (cameraInput != Vector2.zero)
        {
            position.x += cameraInput.x * speed * Time.deltaTime;
            position.z += cameraInput.y * speed * Time.deltaTime;
            var dist = position.magnitude;
            if (dist > radius)
            {
                position *= radius / dist;
            }
            transform.localPosition = position;
        }
    }

    private void OnDestroy()
    {
        InputManager.Instance.OnCameraMove_performed -= CameraMove_performed;
    }
}

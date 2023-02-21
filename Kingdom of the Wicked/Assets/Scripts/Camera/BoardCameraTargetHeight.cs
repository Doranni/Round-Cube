using UnityEngine;

public class BoardCameraTargetHeight : MonoBehaviour
{
    [SerializeField] private float height;

    private Vector3 position;

    void LateUpdate()
    {
        position = transform.position;
        position.y = height;
        transform.position = position;
    }
}

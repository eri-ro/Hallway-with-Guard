using UnityEngine;

public class BillboardTracker : MonoBehaviour
{
    [SerializeField]
    private Camera _mainCamera;

    private void LateUpdate()
    {
        if (_mainCamera == null)
            _mainCamera = Camera.main;

        Vector3 cameraPosition = _mainCamera.transform.position;

        cameraPosition.y = transform.position.y;

        transform.LookAt(cameraPosition);
        transform.Rotate(0f, 180f, 0f);
    }
}
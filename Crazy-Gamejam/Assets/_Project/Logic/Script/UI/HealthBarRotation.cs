using UnityEngine;

public class HealthBarRotation : MonoBehaviour
{
    private Camera _mainCamera;

    private void Start()
    {
        _mainCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (_mainCamera != null) transform.LookAt(transform.position + _mainCamera.transform.forward);
    }
}
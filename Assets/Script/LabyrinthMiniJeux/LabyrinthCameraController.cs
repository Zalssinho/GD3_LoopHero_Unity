using UnityEngine;
using UnityEngine.InputSystem;

public class LabyrinthCameraController : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset = new Vector3(0f, 2f, -4f);

    [Header("Orbit Settings")]
    [SerializeField] private float _mouseSensitivity = 3f;
    [SerializeField] private float _minVerticalAngle = -20f;
    [SerializeField] private float _maxVerticalAngle = 60f;
    [SerializeField] private float _followSmoothness = 10f;

    private float _yaw;
    private float _pitch;

    private void OnEnable()
    {
        _yaw = transform.eulerAngles.y;
        _pitch = transform.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void LateUpdate()
    {
        if (_target == null) return;

        HandleCameraRotation();
        FollowTarget();
    }

    private void HandleCameraRotation()
    {
        Mouse mouse = Mouse.current;
        if (mouse == null) return;

        Vector2 mouseDelta = mouse.delta.ReadValue();
        _yaw += mouseDelta.x * _mouseSensitivity;
        _pitch -= mouseDelta.y * _mouseSensitivity;
        _pitch = Mathf.Clamp(_pitch, _minVerticalAngle, _maxVerticalAngle);
    }

    private void FollowTarget()
    {
        Quaternion rotation = Quaternion.Euler(_pitch, _yaw, 0f);
        Vector3 desiredPos = _target.position + rotation * _offset;

        transform.position = Vector3.Lerp(transform.position, desiredPos, _followSmoothness * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, _followSmoothness * Time.deltaTime);
    }
}

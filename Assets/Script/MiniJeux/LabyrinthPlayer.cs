using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LabyrinthPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _moveSpeed = 5f;
    [SerializeField] private float _gravity = -9.81f;

    private CharacterController _characterController;
    private float _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        // ZQSD (AZERTY) + flèches en fallback
        float horizontal = 0f;
        float vertical   = 0f;

        if (keyboard.qKey.isPressed || keyboard.leftArrowKey.isPressed)  horizontal -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) horizontal += 1f;
        if (keyboard.zKey.isPressed || keyboard.upArrowKey.isPressed)    vertical   += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)  vertical   -= 1f;

        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        // Gravité
        if (_characterController.isGrounded)
            _verticalVelocity = -1f;
        else
            _verticalVelocity += _gravity * Time.deltaTime;

        moveDirection *= _moveSpeed * Time.deltaTime;
        moveDirection.y = _verticalVelocity * Time.deltaTime;

        _characterController.Move(moveDirection);
    }
}

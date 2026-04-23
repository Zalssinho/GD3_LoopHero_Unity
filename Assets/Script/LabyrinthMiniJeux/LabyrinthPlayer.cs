using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class LabyrinthPlayer : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _walkSpeed = 3f;
    [SerializeField] private float _runSpeed = 6f;
    [SerializeField] private float _gravity = -9.81f;
    [SerializeField] private float _rotationSpeed = 12f;

    [Header("Camera Reference")]
    [SerializeField] private Transform _cameraTransform;

    [Header("Animation")]
    [SerializeField] private Animator _animator;

    private static readonly int SpeedHash = Animator.StringToHash("Speed");
    private static readonly int PickUpHash = Animator.StringToHash("PickUp");

    private CharacterController _characterController;
    private float _verticalVelocity;
    private bool _isPickingUp = false;

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
        if (_isPickingUp) return;
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        // AZERTY ZQSD = positions physiques WASD
        float horizontal = 0f;
        float vertical = 0f;

        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed) horizontal -= 1f;
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed) horizontal += 1f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed) vertical += 1f;
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed) vertical -= 1f;

        bool isRunning = keyboard.leftShiftKey.isPressed;

        // Directions relatives à la caméra (sans l'axe Y)
        Vector3 camForward = _cameraTransform.forward;
        Vector3 camRight = _cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 moveDirection = camForward * vertical + camRight * horizontal;
        float inputMagnitude = Mathf.Clamp01(moveDirection.magnitude);

        // Rotation du player vers la direction de déplacement
        if (inputMagnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }

        // Vitesse selon marche / course (Shift)
        float currentSpeed = isRunning ? _runSpeed : _walkSpeed;

        // Paramètre animator : 0 = idle, 0.5 = walk, 1 = run
        float animSpeed = inputMagnitude > 0.1f ? (isRunning ? 1f : 0.5f) : 0f;
        if (_animator != null)
            _animator.SetFloat(SpeedHash, animSpeed, 0.1f, Time.deltaTime);

        // Gravité
        if (_characterController.isGrounded)
            _verticalVelocity = -1f;
        else
            _verticalVelocity += _gravity * Time.deltaTime;

        Vector3 velocity = moveDirection.normalized * (inputMagnitude * currentSpeed) + Vector3.up * _verticalVelocity;
        _characterController.Move(velocity * Time.deltaTime);
    }

    /// <summary>Déclenche l'animation de ramassage et appelle onComplete une fois terminée.</summary>
    public void PlayPickUp(System.Action onComplete)
    {
        if (_isPickingUp) return;
        StartCoroutine(PickUpRoutine(onComplete));
    }

    private System.Collections.IEnumerator PickUpRoutine(System.Action onComplete)
    {
        _isPickingUp = true;

        if (_animator != null)
        {
            _animator.SetFloat(SpeedHash, 0f);
            _animator.SetTrigger(PickUpHash);
        }

        // Attend la durée de l'animation via l'exit time (90% de la clip)
        AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
        float clipDuration = stateInfo.length > 0 ? stateInfo.length * 0.9f : 1.5f;
        yield return new WaitForSeconds(clipDuration);

        _isPickingUp = false;
        onComplete?.Invoke();
    }
}

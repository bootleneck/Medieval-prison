using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(PlayerStamina))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _normalSpeed = 4f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _crouchSpeed = 2f;
    [SerializeField] private float _proneSpeed = 0.8f;

    [Header("Sprint Stamina")]
    [SerializeField] private float _sprintStaminaCostPerSecond = 20f;

    [Header("Gravity & Jump")]
    [SerializeField] private float _gravity = -19.81f;
    [SerializeField] private float _jumpForce = 7f;

    private CharacterController _characterController;
    private PlayerCrouch _crouchScript;
    private PlayerStamina _stamina;

    private Vector2 _move;
    private float _verticalVelocity;

    private bool _isSprinting;    

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _crouchScript = GetComponent<PlayerCrouch>();
        _stamina = GetComponent<PlayerStamina>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        HandleMovement();
    }

    // =========================================================
    // MOVEMENT CORE (UNIFICADO)
    // =========================================================

    private void HandleMovement()
    {
        float baseSpeed = GetBaseSpeed();
        float finalSpeed = ApplySprint(baseSpeed);

        Vector3 moveDirection =
            transform.forward * _move.y +
            transform.right * _move.x;

        Vector3 movement = moveDirection.normalized * finalSpeed;

        // Gravity
        if (_characterController.isGrounded && _verticalVelocity < 0)
        {
            _verticalVelocity = -2f;
        }
        else
        {
            _verticalVelocity += _gravity * Time.deltaTime;
        }

        movement.y = _verticalVelocity;

        _characterController.Move(movement * Time.deltaTime);
    }

    // =========================================================
    // SPEED LOGIC (UNA SOLA FUENTE DE VERDAD)
    // =========================================================

    private float GetBaseSpeed()
    {
        if (_crouchScript != null && _crouchScript.IsLowered)
        {
            return _crouchScript.IsProne ? _proneSpeed : _crouchSpeed;
        }

        return _normalSpeed;
    }

    private float ApplySprint(float baseSpeed)
    {
        bool canSprint =
            _isSprinting &&
            _move != Vector2.zero &&
            (_crouchScript == null || !_crouchScript.IsLowered) &&
            _stamina.HasStamina(0.1f);

        if (canSprint)
        {
            _stamina.UseStamina(_sprintStaminaCostPerSecond * Time.deltaTime);
            return _sprintSpeed;
        }

        return baseSpeed;
    }

    // =========================================================
    // INPUTS
    // =========================================================

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        _isSprinting = context.performed;
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        bool canJump =
            context.performed &&
            _characterController.isGrounded &&
            (_crouchScript == null || !_crouchScript.IsLowered);

        if (canJump)
        {
            _verticalVelocity = _jumpForce;
        }
    }
}
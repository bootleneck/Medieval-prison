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

    private bool _jumpStarted;
    private bool _wasGrounded;
   
    private float _speedMultiplier = 1f;

    public MovementState CurrentState { get; private set; }

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _crouchScript = GetComponent<PlayerCrouch>();
        _stamina = GetComponent<PlayerStamina>();
    }

    private void Start()
    {
        _wasGrounded = _characterController.isGrounded;
    }

    private void Update()
    {
        HandleMovement();
        UpdateState();
        CheckLanding();
    }

    private void HandleMovement()
    {
        float baseSpeed = GetBaseSpeed();
        float finalSpeed = ApplySprint(baseSpeed);

        Vector3 moveDirection =
            transform.forward * _move.y +
            transform.right * _move.x;

        Vector3 movement = moveDirection.normalized * finalSpeed;
       
        if (_characterController.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = -2f;
        else
            _verticalVelocity += _gravity * Time.deltaTime;

        movement.y = _verticalVelocity;

        _characterController.Move(movement * Time.deltaTime);
    }

    private void UpdateState()
    {
        if (_crouchScript != null && _crouchScript.IsProne)
        {
            CurrentState = MovementState.Prone;
            return;
        }

        if (_crouchScript != null && _crouchScript.IsLowered)
        {
            CurrentState = MovementState.Crouch;
            return;
        }

        if (IsActuallySprinting)
        {
            CurrentState = MovementState.Sprint;
            return;
        }

        CurrentState = MovementState.Walk;
    }

    private float GetBaseSpeed()
    {
        float speed;

        if (_crouchScript != null && _crouchScript.IsLowered)
            speed = _crouchScript.IsProne ? _proneSpeed : _crouchSpeed;
        else
            speed = _normalSpeed;

        return speed;
    }

    private float ApplySprint(float baseSpeed)
    {
        float speed = baseSpeed * _speedMultiplier;

        if (IsActuallySprinting)
        {
            _stamina.UseStamina(_sprintStaminaCostPerSecond * Time.deltaTime);
            return _sprintSpeed * _speedMultiplier;
        }

        return speed;
    }

    public bool IsActuallySprinting =>
        _isSprinting &&
        _move.sqrMagnitude > 0.01f &&
        (_crouchScript == null || !_crouchScript.IsLowered) &&
        _stamina.HasStamina(0.1f);

    public bool IsMoving =>
        _move.sqrMagnitude > 0.01f;

    private void CheckLanding()
    {
        if (_jumpStarted &&
            !_wasGrounded &&
            _characterController.isGrounded)
        {
            AudioManager.Instance.PlaySFX3D("land", transform.position);
            _jumpStarted = false;
        }

        _wasGrounded = _characterController.isGrounded;
    }   

    public void SetSpeedMultiplier(float multiplier)
    {
        _speedMultiplier = Mathf.Clamp(multiplier, 0.1f, 1f);
    }

    public void ResetSpeedMultiplier()
    {
        _speedMultiplier = 1f;
    }    

    public void OnMove(InputAction.CallbackContext context)
        => _move = context.ReadValue<Vector2>();

    public void OnSprint(InputAction.CallbackContext context)
        => _isSprinting = context.performed;

    public void OnJump(InputAction.CallbackContext context)
    {
        if (!context.performed)
            return;

        if (!_characterController.isGrounded)
            return;

        if (_jumpStarted)
            return;

        // No permitir saltar agachado o cuerpo a tierra
        if (_crouchScript != null && _crouchScript.IsLowered)
            return;

        _jumpStarted = true;
        _verticalVelocity = _jumpForce;

        AudioManager.Instance.PlaySFX3D("jump", transform.position);
    }
}
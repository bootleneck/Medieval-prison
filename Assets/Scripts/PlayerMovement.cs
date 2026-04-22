using UnityEngine;
using UnityEngine.InputSystem;

using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    
    [Header("Movement Settings")]
    [SerializeField] private float _normalSpeed = 4f;
    [SerializeField] private float _sprintSpeed = 8f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 20f;

    [Header("Gravity & Jump")]
    [SerializeField] private float _gravity = -19.81f;
    [SerializeField] private float _jumpForce = 7f;

    [Header("Agachado")]
    [SerializeField] private float _crouchHeight = 0.1f;
    [SerializeField] private float _standingHeight = 2f;
    [SerializeField] private float _crouchSpeed = 2.5f;

    private CharacterController _characterController;
    private float _speed;
    private Vector2 _move;
    private float _verticalVelocity;
    private bool _isCrouching = false;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        _speed = _normalSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.forward * _move.y + transform.right * _move.x;
        Vector3 movement = moveDirection.normalized * _speed;

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

    public void OnMove(InputAction.CallbackContext context)
    {
        _move = context.ReadValue<Vector2>();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // RESTRICCIÓN: Solo corre si NO está agachado
        if (context.performed && !_isCrouching)
        {
            _speed = _sprintSpeed;
        }
        else if (context.canceled)
        {
            // Si deja de apretar Shift, volvemos a la velocidad que corresponda
            _speed = _isCrouching ? _crouchSpeed : _normalSpeed;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        //  No saltar si está agachado 
        if (context.performed && _characterController.isGrounded && !_isCrouching)
        {
            _verticalVelocity = _jumpForce;
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _isCrouching = true; // Estado: Agachado
            _characterController.height = _crouchHeight;
            _speed = _crouchSpeed;
        }
        else if (context.canceled)
        {
            _isCrouching = false; // Estado: Parado
            _characterController.height = _standingHeight;
            _speed = _normalSpeed;
        }
    }
}


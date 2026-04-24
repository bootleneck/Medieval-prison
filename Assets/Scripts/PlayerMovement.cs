using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]  // Ajustes de velocidad para cada estado
    [SerializeField] private float _normalSpeed = 4f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _crouchSpeed = 2f; 
    [SerializeField] private float _proneSpeed = 0.8f; 

    [Header("Gravity & Jump")]
    [SerializeField] private float _gravity = -19.81f;
    [SerializeField] private float _jumpForce = 7f;

    private CharacterController _characterController;
    private PlayerCrouch _crouchScript;
    private float _speed;
    private Vector2 _move;
    private float _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _crouchScript = GetComponent<PlayerCrouch>();
    }

    void Start()
    {
        _speed = _normalSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        UpdateSpeed();
        HandleMovement();

    }

    private void UpdateSpeed()
    {
        // Si el script de agachado nos dice que estamos en un estado "bajo"
        if (_crouchScript != null && _crouchScript.IsLowered)
        {
            // Si estamos arrastrados (Prone) usamos _proneSpeed 
            // si no (estamos agachados) usamos _crouchSpeed
            _speed = _crouchScript.IsProne ? _proneSpeed : _crouchSpeed;
        }
        else if (!Keyboard.current.leftShiftKey.isPressed)
        {
            // Solo volvemos a normal si no estamos apretando Shift
            _speed = _normalSpeed;
        }
    }

    private void HandleMovement()
    {
        Vector3 moveDirection = transform.forward * _move.y + transform.right * _move.x;
        Vector3 movement = moveDirection.normalized * _speed;

        if (_characterController.isGrounded && _verticalVelocity < 0)
            _verticalVelocity = -2f;
        else
            _verticalVelocity += _gravity * Time.deltaTime;

        movement.y = _verticalVelocity;
        _characterController.Move(movement * Time.deltaTime);
    }

    // --- INPUT EVENTS ---

    public void OnMove(InputAction.CallbackContext context) => _move = context.ReadValue<Vector2>();

    public void OnSprint(InputAction.CallbackContext context)
    {
        // Solo permite correr si el cuerpo está totalmente erguido
        if (context.performed && (_crouchScript == null || !_crouchScript.IsLowered))
        {
            _speed = _sprintSpeed;
        }
        else if (context.canceled)
        {
            // Al soltar shift, regresamos a la velocidad que corresponda al estado actual
            if (_crouchScript != null && _crouchScript.IsLowered)
                _speed = _crouchScript.IsProne ? _proneSpeed : _crouchSpeed;
            else
                _speed = _normalSpeed;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && _characterController.isGrounded && (_crouchScript == null || !_crouchScript.IsLowered))
        {
            _verticalVelocity = _jumpForce;
        }
    }
}




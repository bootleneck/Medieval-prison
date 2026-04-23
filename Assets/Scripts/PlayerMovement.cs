using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _normalSpeed = 4f;
    [SerializeField] private float _sprintSpeed = 8f;
    [SerializeField] private float _crouchSpeed = 2.5f; // La mantenemos para saber a qué velocidad ir

    [Header("Gravity & Jump")]
    [SerializeField] private float _gravity = -19.81f;
    [SerializeField] private float _jumpForce = 7f;

    private CharacterController _characterController;
    private PlayerCrouch _crouchScript; // Referencia al nuevo script
    private float _speed;
    private Vector2 _move;
    private float _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _crouchScript = GetComponent<PlayerCrouch>(); // Obtenemos el script de agachado
    }

    void Start()
    {
        _speed = _normalSpeed;
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Actualizamos la velocidad dependiendo de si el otro script dice que estamos agachados
        UpdateSpeed();
        HandleMovement();
    }

    private void UpdateSpeed()
    {
        if (_crouchScript != null && _crouchScript.IsCrouching)
        {
            _speed = _crouchSpeed;
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
        // Solo permite sprintar si NO estamos agachados según el otro script
        if (context.performed && (_crouchScript == null || !_crouchScript.IsCrouching))
        {
            _speed = _sprintSpeed;
        }
        else if (context.canceled)
        {
            _speed = (_crouchScript != null && _crouchScript.IsCrouching) ? _crouchSpeed : _normalSpeed;
        }
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        // Solo salta si no estamos agachados
        if (context.performed && _characterController.isGrounded && (_crouchScript == null || !_crouchScript.IsCrouching))
        {
            _verticalVelocity = _jumpForce;
        }
    }
}




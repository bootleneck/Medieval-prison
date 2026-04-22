using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float _normalSpeed = 5f;
    [SerializeField] private float _sprintSpeed = 10f;

    [Header("Rotation")]
    [SerializeField] private float _rotationSpeed = 20f; 

    [Header("Gravity & Jump")]
    [SerializeField] private float _gravity = -19.81f; 
    [SerializeField] private float _jumpForce = 7f;

    [Header("Agachado")]
    [SerializeField] private float _crouchHeight = 1f;
    [SerializeField] private float _standingHeight = 2f;
    [SerializeField] private float _crouchSpeed = 2.5f; // El personaje camina más lento agachado

    private CharacterController _characterController;
    private float _speed;
    private Vector2 _move;
    private float _verticalVelocity;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        _speed = _normalSpeed;
        Cursor.lockState = CursorLockMode.Locked; // Bloquea el mouse al centro
 
    }

    void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        // Movimiento basado en hacia donde mira el cuerpo del jugador
        Vector3 moveDirection = transform.forward * _move.y + transform.right * _move.x;

        
        Vector3 movement = moveDirection.normalized * _speed;

        // Lógica de Gravedad
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
        if (context.performed) _speed = _sprintSpeed;
        if (context.canceled) _speed = _normalSpeed;
    }

    // Salto
    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && _characterController.isGrounded)
        {
            _verticalVelocity = _jumpForce;
        }
    }
    
    public void OnCrouch(InputAction.CallbackContext context)
    {
        Debug.Log("Sistema de Input: Crouch detectado en fase " + context.phase);

        if (context.performed)
        {
            _characterController.height = _crouchHeight;
            _speed = _crouchSpeed;
           
        }
        else if (context.canceled)
        {
            _characterController.height = _standingHeight;
            _speed = _normalSpeed;
            
        }
    }
}


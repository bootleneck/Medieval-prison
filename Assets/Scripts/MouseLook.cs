using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 25f;
    [SerializeField] private Transform _playerBody; // Arrastrá aquí al objeto "Player"

    private float _xRotation = 0f;
    private PlayerInput _playerInput;
    private InputAction _lookAction;

    void Awake()
    {
        // Buscamos el componente PlayerInput que está en el padre
        _playerInput = GetComponentInParent<PlayerInput>();
        _lookAction = _playerInput.actions["Look"];
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Sincronización inicial para evitar el "salto" de cámara
        _xRotation = transform.localEulerAngles.x;
        if (_xRotation > 180) _xRotation -= 360;
    }

    void Update()
    {
        Vector2 lookInput = _lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * _sensitivity * Time.deltaTime;
        float mouseY = lookInput.y * _sensitivity * Time.deltaTime;

        // Rotación Vertical (Cámara)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        // Rotación Horizontal (Cuerpo)
        // Hacemos que la cámara le diga al cuerpo que gire
        _playerBody.Rotate(Vector3.up * mouseX);
    }
}

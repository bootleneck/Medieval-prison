using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 25f;
    [SerializeField] private Transform _playerBody; // Arrastra aquí al objeto "Player" (la cápsula)

    private float _xRotation = 0f;
    private PlayerInput _playerInput;
    private InputAction _lookAction;
    private float _sensitivityMultiplier = 1f;

    void Awake()
    {
        // Buscamos el componente PlayerInput que está en el personaje
        _playerInput = GetComponentInParent<PlayerInput>();
        if (_playerInput != null)
        {
            _lookAction = _playerInput.actions["Look"];
        }
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
        if (_lookAction == null) return;

        // Lee en cada frame el valor guardado por el slider de pausa
        _sensitivityMultiplier = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        Vector2 lookInput = _lookAction.ReadValue<Vector2>();

        // Multiplicamos los valores de entrada por la sensibilidad base y el multiplicador del menú
        float mouseX = lookInput.x * _sensitivity * _sensitivityMultiplier * Time.deltaTime;
        float mouseY = lookInput.y * _sensitivity * _sensitivityMultiplier * Time.deltaTime;

        // Rotación Vertical (La cabeza/Main Camera)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        // Rotación Horizontal (El cuerpo/Cápsula)
        if (_playerBody != null)
        {
            _playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
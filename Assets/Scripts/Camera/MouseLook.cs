using UnityEngine;
using UnityEngine.InputSystem;

// Colocar este script en el objeto "Eyes" (hijo de Player)
public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 25f;
    [SerializeField] private Transform _playerBody; // Arrastrá el objeto "Player" aquí

    private float _xRotation = 0f;
    private PlayerInput _playerInput;
    private InputAction _lookAction;
    private float _sensitivityMultiplier = 1f;

    void Awake()
    {
        // Eyes es hijo de Player, entonces GetComponentInParent encuentra el PlayerInput del Player
        _playerInput = GetComponentInParent<PlayerInput>();
        if (_playerInput != null)
        {
            _lookAction = _playerInput.actions["Look"];
        }
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        _xRotation = transform.localEulerAngles.x;
        if (_xRotation > 180) _xRotation -= 360;
    }

    void Update()
    {
        if (_lookAction == null) return;

        _sensitivityMultiplier = PlayerPrefs.GetFloat("MouseSensitivity", 1f);

        Vector2 lookInput = _lookAction.ReadValue<Vector2>();

        float mouseX = lookInput.x * _sensitivity * _sensitivityMultiplier * Time.deltaTime;
        float mouseY = lookInput.y * _sensitivity * _sensitivityMultiplier * Time.deltaTime;

        // Rotación vertical: rota Eyes (y con él el VirtualCamera_POV)
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);

        // Rotación horizontal: rota el cuerpo del Player
        if (_playerBody != null)
        {
            _playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
    [SerializeField] private float _sensitivity = 25f;
    [SerializeField] private Transform _playerBody;

    private float _xRotation = 0f;
    private PlayerInput _playerInput;
    private InputAction _lookAction;
    private float _sensitivityMultiplier = 1f;

    void Awake()
    {        
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
        
        _xRotation -= mouseY;
        _xRotation = Mathf.Clamp(_xRotation, -80f, 80f);
        transform.localRotation = Quaternion.Euler(_xRotation, 0, 0);
        
        if (_playerBody != null)
        {
            _playerBody.Rotate(Vector3.up * mouseX);
        }
    }
}
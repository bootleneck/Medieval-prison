using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    [Header("Configuración de Alturas")]
    [SerializeField] private float _crouchHeight = 1.6f;
    [SerializeField] private float _standingHeight = 2.0f;
    [SerializeField] private float _camCrouchY = 1.3f;
    [SerializeField] private float _crouchSmoothTime = 10f; // Velocidad del suavizado

    [Header("Detección de Techo (Raycast)")]
    [SerializeField] private LayerMask _ceilingLayer;
    [SerializeField] private float _rayDistance = 1.5f;

    private CharacterController _controller;
    private Camera _cam;
    private float _standCamY;
    private float _targetCamY; // Objetivo de altura para la cámara
    private bool _isCrouching;
    private bool _wantsToStandUp;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _cam = GetComponentInChildren<Camera>();

        if (_cam != null)
        {
            _standCamY = _cam.transform.localPosition.y;
            _targetCamY = _standCamY; // Iniciamos con el objetivo en altura normal
        }
    }

    private void Update()
    {
        // 1. Suavizado de la cámara (Lerp)
        if (_cam != null)
        {
            float currentY = _cam.transform.localPosition.y;
            float newY = Mathf.Lerp(currentY, _targetCamY, Time.deltaTime * _crouchSmoothTime);
            _cam.transform.localPosition = new Vector3(0, newY, 0);
        }

        // 2. Verificación de techo si el jugador intentó pararse
        if (_wantsToStandUp)
        {
            CheckIfCanStandUp();
        }
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _wantsToStandUp = false;
            PerformCrouch(true);
        }
        else if (context.canceled)
        {
            _wantsToStandUp = true;
            CheckIfCanStandUp();
        }
    }

    private void CheckIfCanStandUp()
    {
        // 🧩 RAYCAST: Verificamos espacio sobre la cabeza antes de pararnos
        if (!Physics.Raycast(transform.position, Vector3.up, _rayDistance, _ceilingLayer))
        {
            PerformCrouch(false);
            _wantsToStandUp = false;
        }
        else
        {
            Debug.Log("No puedes pararte: hay un objeto encima");
        }
    }

    private void PerformCrouch(bool crouch)
    {
        _isCrouching = crouch;
        _controller.height = crouch ? _crouchHeight : _standingHeight;
        _controller.center = new Vector3(0, _controller.height / 2f, 0);

        // Ya no movemos la cámara aquí, solo definimos a dónde debe ir (Update hará el resto)
        _targetCamY = crouch ? _camCrouchY : _standCamY;
    }

    public bool IsCrouching => _isCrouching;
}
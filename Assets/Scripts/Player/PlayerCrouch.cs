using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    public enum CrouchState
    {
        Standing,
        Crouching,
        Prone
    }

    private CrouchState _currentState = CrouchState.Standing;

    [Header("Configuración de Alturas")]
    [SerializeField] private float _standHeight = 2.0f;
    [SerializeField] private float _crouchHeight = 1.2f;
    [SerializeField] private float _proneHeight = 0.6f;

    [Header("Altura de Cámara")]
    [SerializeField] private float _camStandY = 1.6f;
    [SerializeField] private float _camCrouchY = 1.0f;
    [SerializeField] private float _camProneY = 0.45f;

    [Header("Suavizado")]
    [SerializeField] private float _smoothSpeed = 10f;

    [Header("Detección de Techo")]
    [SerializeField] private LayerMask _ceilingLayer;
    [SerializeField] private float _spherePadding = 0.9f;

    private CharacterController _controller;
    private Camera _cam;

    private float _targetHeight;
    private float _targetCamY;

    private bool _wantsToRise;

    // =========================
    // INICIO
    // =========================

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _cam = GetComponentInChildren<Camera>();

        // Mantiene altura original del controller
        _targetHeight = _standHeight;

        // Guarda la posición REAL de la cámara
        if (_cam != null)
        {
            _camStandY = _cam.transform.localPosition.y;

            // Ajusta crouch y prone relativos
            _camCrouchY = _camStandY - 0.7f;
            _camProneY = _camStandY - 1.9f;

            _targetCamY = _camStandY;
        }

        _controller.height = _standHeight;

        Vector3 center = _controller.center;
        center.y = _standHeight / 2f;
        _controller.center = center;
    }

    // =========================
    // UPDATE
    // =========================

    private void Update()
    {
        SmoothCharacterHeight();
        SmoothCameraHeight();

        if (_wantsToRise)
        {
            CheckIfCanRise();
        }
    }

    // =========================
    // INPUTS
    // =========================

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        _wantsToRise = false;

        switch (_currentState)
        {
            case CrouchState.Standing:
                SetState(CrouchState.Crouching);
                break;

            case CrouchState.Crouching:
                SetState(CrouchState.Prone);
                break;

            case CrouchState.Prone:
                _wantsToRise = true;
                break;
        }
    }

    public void OnProne(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _wantsToRise = false;
            SetState(CrouchState.Prone);
        }

        if (context.canceled)
        {
            _wantsToRise = true;
        }
    }

    // =========================
    // CAMBIO DE ESTADOS
    // =========================

    private void SetState(CrouchState state)
    {
        _currentState = state;

        switch (state)
        {
            case CrouchState.Standing:
                _targetHeight = _standHeight;
                _targetCamY = _camStandY;
                break;

            case CrouchState.Crouching:
                _targetHeight = _crouchHeight;
                _targetCamY = _camCrouchY;
                break;

            case CrouchState.Prone:
                _targetHeight = _proneHeight;
                _targetCamY = _camProneY;
                break;
        }
    }

    // =========================
    // SUAVIZADO DEL COLLIDER
    // =========================

    private void SmoothCharacterHeight()
    {
        float newHeight = Mathf.Lerp(
            _controller.height,
            _targetHeight,
            Time.deltaTime * _smoothSpeed
        );

        newHeight = Mathf.Clamp(
            newHeight,
            _proneHeight,
            _standHeight
        );

        _controller.enabled = false;

        _controller.height = newHeight;

        // Mantiene X y Z originales
        Vector3 center = _controller.center;
        center.y = newHeight / 2f;
        _controller.center = center;

        _controller.enabled = true;
    }

    // =========================
    // SUAVIZADO DE CÁMARA
    // =========================

    private void SmoothCameraHeight()
    {
        if (_cam == null) return;

        float newY = Mathf.Lerp(
            _cam.transform.localPosition.y,
            _targetCamY,
            Time.deltaTime * _smoothSpeed
        );

        Vector3 camPos = _cam.transform.localPosition;
        camPos.y = newY;

        _cam.transform.localPosition = camPos;
    }

    // =========================
    // DETECCIÓN DE TECHO
    // =========================

    private void CheckIfCanRise()
    {
        CrouchState nextState;
        float targetHeight;

        if (_currentState == CrouchState.Prone)
        {
            nextState = CrouchState.Crouching;
            targetHeight = _crouchHeight;
        }
        else
        {
            nextState = CrouchState.Standing;
            targetHeight = _standHeight;
        }

        Vector3 origin = transform.position + Vector3.up * (_controller.height / 2f);

        float castDistance = targetHeight - _controller.height;

        bool blocked = Physics.SphereCast(
            origin,
            _controller.radius * _spherePadding,
            Vector3.up,
            out RaycastHit hit,
            castDistance,
            _ceilingLayer
        );

        Debug.DrawRay(
            origin,
            Vector3.up * castDistance,
            blocked ? Color.red : Color.green
        );

        if (!blocked)
        {
            SetState(nextState);

            if (nextState == CrouchState.Standing)
            {
                _wantsToRise = false;
            }
        }
    }

    // =========================
    // PROPIEDADES
    // =========================

    public bool IsLowered => _currentState != CrouchState.Standing;

    public bool IsProne => _currentState == CrouchState.Prone;

    public bool IsCrouching => _currentState == CrouchState.Crouching;

    public CrouchState CurrentState => _currentState;
}
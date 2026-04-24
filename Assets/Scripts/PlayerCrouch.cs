using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    public enum CrouchState { Standing, Crouching, Prone }
    private CrouchState _currentState = CrouchState.Standing;

    [Header("Configuración de Alturas (Cápsula)")]
    [SerializeField] private float _standHeight = 2.0f;
    [SerializeField] private float _crouchHeight = 1.0f;
    [SerializeField] private float _proneHeight = 0.5f;

    [Header("Alturas de Cámara (Visual)")]
    [SerializeField] private float _camStandY = 1.6f;
    [SerializeField] private float _camCrouchY = 0.8f;
    [SerializeField] private float _camProneY = 0.3f;
    [SerializeField] private float _smoothTime = 10f;

    [Header("Detección de Techo (Raycast)")]
    [SerializeField] private LayerMask _ceilingLayer;
    [SerializeField] private float _rayOffset = 0.25f;

    private CharacterController _controller;
    private Camera _cam;
    private float _targetHeight;
    private float _targetCamY;
    private bool _wantsToRise = false;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _cam = GetComponentInChildren<Camera>();
        _targetHeight = _standHeight;
        if (_cam != null) _targetCamY = _cam.transform.localPosition.y;
    }

    private void Update()
    {
        // 1. Suavizado de la cápsula y la cámara
        _controller.height = Mathf.Lerp(_controller.height, _targetHeight, Time.deltaTime * _smoothTime);
        _controller.center = new Vector3(0, _controller.height / 2f, 0);

        if (_cam != null)
        {
            float newY = Mathf.Lerp(_cam.transform.localPosition.y, _targetCamY, Time.deltaTime * _smoothTime);
            _cam.transform.localPosition = new Vector3(0, newY, 0);
        }

        // 2. Si soltamos tecla bajo techo, el Raycast decide cuándo subir
        if (_wantsToRise) CheckIfCanRise();
    }

    public void OnCrouch(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _wantsToRise = false;
            if (_currentState == CrouchState.Standing) SetCrouchState(CrouchState.Crouching);
            else if (_currentState == CrouchState.Crouching) SetCrouchState(CrouchState.Prone);
            else _wantsToRise = true;
        }
    }

    public void OnProne(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _wantsToRise = false;
            SetCrouchState(CrouchState.Prone);
        }
        else if (context.canceled)
        {
            _wantsToRise = true;
        }
    }

    private void SetCrouchState(CrouchState newState)
    {
        _currentState = newState;
        switch (newState)
        {
            case CrouchState.Standing: _targetHeight = _standHeight; _targetCamY = _camStandY; break;
            case CrouchState.Crouching: _targetHeight = _crouchHeight; _targetCamY = _camCrouchY; break;
            case CrouchState.Prone: _targetHeight = _proneHeight; _targetCamY = _camProneY; break;
        }
    }

    private void CheckIfCanRise()
    {
        float targetH = (_currentState == CrouchState.Prone) ? _crouchHeight : _standHeight;
        Vector3 origin = transform.position + Vector3.up * (_controller.height - 0.1f);
        float dist = (targetH - _controller.height) + 0.2f;

        Vector3[] origins = { origin, origin + transform.forward * _rayOffset, origin - transform.forward * _rayOffset };

        bool blocked = false;
        foreach (var pos in origins)
        {
            Debug.DrawRay(pos, Vector3.up * dist, Color.red);
            if (Physics.Raycast(pos, Vector3.up, dist, _ceilingLayer)) { blocked = true; break; }
        }

        if (!blocked)
        {
            if (_currentState == CrouchState.Prone) SetCrouchState(CrouchState.Crouching);
            else { SetCrouchState(CrouchState.Standing); _wantsToRise = false; }
        }
    }

    // Propiedades para que PlayerMovement las lea
    public bool IsLowered => _currentState != CrouchState.Standing;
    public bool IsProne => _currentState == CrouchState.Prone;
}
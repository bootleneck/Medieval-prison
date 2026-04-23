using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    [Header("Configuración de Alturas")]
    [SerializeField] private float _crouchHeight = 1f;
    [SerializeField] private float _standingHeight = 2.0f;
    [SerializeField] private float _camCrouchY = 0.8f;
    [SerializeField] private float _crouchSmoothTime = 10f;

    [Header("Detección de Techo (Raycast)")]
    [SerializeField] private LayerMask _ceilingLayer;
    [SerializeField] private float _rayOffset = 0.3f; // Distancia de los rayos laterales

    private CharacterController _controller;
    private Camera _cam;
    private float _standCamY;
    private float _targetCamY;
    private bool _isCrouching;
    private bool _wantsToStandUp;

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _cam = GetComponentInChildren<Camera>();

        if (_cam != null)
        {
            _standCamY = _cam.transform.localPosition.y;
            _targetCamY = _standCamY;
        }
    }

    private void Update()
    {
        // 1. Suavizado de la cámara (Lerp)
        if (_cam != null)
        {
            float newY = Mathf.Lerp(_cam.transform.localPosition.y, _targetCamY, Time.deltaTime * _crouchSmoothTime);
            _cam.transform.localPosition = new Vector3(0, newY, 0);
        }

        // 2. Lógica de seguridad constante
        if (_isCrouching && _wantsToStandUp)
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
        // El origen es la parte superior de la cabeza actual
        Vector3 origin = transform.position + Vector3.up * _controller.height;
        float distance = (_standingHeight - _controller.height) + 0.2f;

        //  5 puntos de origen (Centro + Cruz)
        Vector3[] rayOrigins = new Vector3[]
        {
            origin,
            origin + new Vector3(_rayOffset, 0, 0),
            origin + new Vector3(-_rayOffset, 0, 0),
            origin + new Vector3(0, 0, _rayOffset),
            origin + new Vector3(0, 0, -_rayOffset)
        };

        bool hitSomething = false;

        foreach (Vector3 pos in rayOrigins)
        {
            // Dibujamos los rayos para Debug 
            Debug.DrawRay(pos, Vector3.up * distance, Color.red);

            if (Physics.Raycast(pos, Vector3.up, distance, _ceilingLayer))
            {
                hitSomething = true;
                break;
            }
        }

        if (!hitSomething)
        {
            PerformCrouch(false);
            _wantsToStandUp = false;
            Debug.Log("Raycasts despejados: El personaje se para.");
        }
        else
        {
            Debug.Log("Raycast detectó obstrucción: No puedes pararte.");
        }
    }

    private void PerformCrouch(bool crouch)
    {
        _isCrouching = crouch;
        _controller.height = crouch ? _crouchHeight : _standingHeight;
        _controller.center = new Vector3(0, _controller.height / 2f, 0);
        _targetCamY = crouch ? _camCrouchY : _standCamY;
    }

    public bool IsCrouching => _isCrouching;
}
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement movement;

    [Header("Step Timing")]
    [SerializeField] private float walkInterval = 0.5f;
    [SerializeField] private float sprintInterval = 0.3f;
    [SerializeField] private float crouchInterval = 0.7f;
    [SerializeField] private float proneInterval = 1.0f;

    private CharacterController controller;

    private float stepTimer;

    private MovementState lastState;

    private Vector3 lastPosition;

    // índices por estado
    private int walkIndex;
    private int sprintIndex;
    private int crouchIndex;
    private int proneIndex;

    // sonidos
    private string[] walkSteps = { "step_walk_1", "step_walk_2" };
    private string[] sprintSteps = { "step_sprint_1", "step_sprint_2" };
    private string[] crouchSteps = { "step_crouch_1", "step_crouch_2" };
    private string[] proneSteps = { "step_prone_1", "step_prone_2" };

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
        lastState = movement.CurrentState;
        lastPosition = transform.position;
    }

    private void Update()
    {
        HandleStateChange();
        HandleFootsteps();
    }

    // =====================================================
    // CAMBIO DE ESTADO
    // =====================================================

    private void HandleStateChange()
    {
        if (movement.CurrentState == lastState)
            return;

        lastState = movement.CurrentState;

        // reset para evitar mezcla
        stepTimer = 0f;

        walkIndex = 0;
        sprintIndex = 0;
        crouchIndex = 0;
        proneIndex = 0;
    }

    // =====================================================
    // FOOTSTEPS
    // =====================================================

    private void HandleFootsteps()
    {
        if (!controller.isGrounded)
            return;

        Vector3 horizontalVelocity = controller.velocity;
        horizontalVelocity.y = 0f;

        // 🔥 si realmente está quieto
        if (horizontalVelocity.sqrMagnitude < 0.05f)
            return;

        stepTimer -= Time.deltaTime;

        if (stepTimer <= 0f)
        {
            PlayFootstep();
            stepTimer = GetInterval();
        }
    }

    private float GetInterval()
    {
        return movement.CurrentState switch
        {
            MovementState.Sprint => sprintInterval,
            MovementState.Crouch => crouchInterval,
            MovementState.Prone => proneInterval,
            _ => walkInterval
        };
    }

    private void PlayFootstep()
    {
        string clip = movement.CurrentState switch
        {
            MovementState.Sprint => GetNext(sprintSteps, ref sprintIndex),
            MovementState.Crouch => GetNext(crouchSteps, ref crouchIndex),
            MovementState.Prone => GetNext(proneSteps, ref proneIndex),
            _ => GetNext(walkSteps, ref walkIndex),
        };

        AudioManager.Instance.PlaySFX3D(clip, transform.position);
    }

    private string GetNext(string[] array, ref int index)
    {
        string clip = array[index];
        index = (index + 1) % array.Length;
        return clip;
    }
}
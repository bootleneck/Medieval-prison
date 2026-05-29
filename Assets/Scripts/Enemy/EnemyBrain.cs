using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;

    public EnemyMovement movement;
    public EnemyStun stun;
    public EnemyMeleeAttack attack;
    public Animator animator;
    private Health health;
    private EnemyAudio enemyAudio;

    [Header("Stats")]
    public float detectionRange = 10f;
    public float attackRange = 2f;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 90f;
    public LayerMask obstacleMask;

    private EnemyState currentState;

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        stun = GetComponent<EnemyStun>();
        attack = GetComponent<EnemyMeleeAttack>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        enemyAudio = GetComponent<EnemyAudio>();
    }

    private void OnEnable()
    {
        if (health != null) health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        if (health != null) health.OnDeath -= HandleDeath;
    }

    private void Start()
    {
        ChangeState(new PatrolState());
    }

    private void Update()
    {
        if (health != null && health.IsDead) return;

        stun.Tick(Time.deltaTime);

        if (stun.IsStunned)
        {
            if (currentState is not StunnedState)
                ChangeState(new StunnedState());

            animator.SetBool("IsMoving", false);
            return;
        }

        currentState?.Update(this);

        UpdateAnimation();
    }

    private void HandleDeath()
    {
        ChangeState(new DeadState());
    }

    // =========================================================
    // ANIMACIÓN SIMPLE Y ROBUSTA
    // =========================================================

    private void UpdateAnimation()
    {
        bool isMoving = movement.IsMovingTowardsTarget();
        animator.SetBool("IsMoving", isMoving);
    }

    // =========================================================
    // VISIÓN
    // =========================================================

    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance > viewDistance)
            return false;

        Vector3 dirNormalized = dirToPlayer.normalized;
        float angle = Vector3.Angle(transform.forward, dirNormalized);

        if (angle > viewAngle * 0.5f)
            return false;

        if (Physics.Raycast(
            transform.position + Vector3.up,
            dirNormalized,
            distance,
            obstacleMask))
        {
            return false;
        }

        return true;
    }

    public void ChangeState(EnemyState newState)
    {
        if (currentState != null &&
            currentState.GetType() == newState.GetType())
        {
            return;
        }

        currentState?.Exit(this);
        currentState = newState;

        // =====================================================
        // CONTROL DE AUDIO POR ESTADOS (CON LOS NUEVOS NOMBRES)
        // =====================================================
        if (enemyAudio != null)
        {
            // Si entra en persecución, suena el loop de persecución
            if (newState is ChaseState)
            {
                enemyAudio.PlayChaseLoop();
            }
            // Si pasa a cualquier otro estado, el loop de persecución se apaga
            else if (newState is PatrolState || newState is DeadState || newState is StunnedState)
            {
                enemyAudio.StopChaseLoop();
            }
        }

        currentState.Enter(this);
    }
}
using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;

    public EnemyMovement movement;
    public EnemyStun stun;
    public EnemyMeleeAttack attack;
    public Animator animator;
    private Health health; // ← 1. Añadimos la referencia de vida aquí

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
        health = GetComponent<Health>(); // ← 2. Buscamos el componente Health aquí
    }

    // ← 3. Añadimos estos dos métodos nuevos obligatorios para escuchar el evento de muerte
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
        // ← 4. Si ya está muerto, salimos del Update de inmediato para congelar la IA
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

    // ← 5. Añadimos este método nuevo que se ejecuta automáticamente al morir
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
        currentState?.Exit(this);
        currentState = newState;
        currentState.Enter(this);
    }
}
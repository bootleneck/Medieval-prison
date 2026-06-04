using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [Header("Refs")]
    public Transform player; // Se completará automáticamente por script

    public EnemyMovement movement;
    public EnemyStun stun;
    public EnemyMeleeAttack meleeAttack;
    public EnemyRangedAttack rangedAttack;
    public Animator animator;
    private Health health;
    private EnemyAudio enemyAudio;

    [Header("Stats")]
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float rangedAttackRange = 10f;

    [Header("Vision")]
    [SerializeField] private float viewDistance = 10f;
    [SerializeField] private float viewAngle = 90f;
    public LayerMask obstacleMask;

    private EnemyState currentState;

    // 🔒 Lock para ataques
    public bool IsAttacking { get; private set; }

    public void SetAttacking(bool value)
    {
        IsAttacking = value;
    }

    private void Awake()
    {
        movement = GetComponent<EnemyMovement>();
        stun = GetComponent<EnemyStun>();
        meleeAttack = GetComponent<EnemyMeleeAttack>();
        rangedAttack = GetComponent<EnemyRangedAttack>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        enemyAudio = GetComponent<EnemyAudio>();
    }

    // Método OnEnable unificado y corregido
    private void OnEnable()
    {
        // SOLUCIÓN DEFINITIVA: Busca en la escena el script único de movimiento del jugador
        // Reemplaza 'PlayerMovement' si tu clase de movimiento se llama de otra forma (ej: PlayerController)
        PlayerMovement jugadorMovimiento = FindAnyObjectByType<PlayerMovement>();

        if (jugadorMovimiento != null)
        {
            // Asignamos el Transform del objeto del jugador de forma infalible
            player = jugadorMovimiento.transform;
        }
        else
        {
            Debug.LogWarning($"[EnemyBrain] {gameObject.name} no pudo encontrar el componente 'PlayerMovement' en la escena.");
        }

        // Suscripción al evento de muerte propio del enemigo
        if (health != null)
            health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnDeath -= HandleDeath;
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
        CombatMusicController.Instance?.UnregisterEnemyCombat(this);
    }

    private void UpdateAnimation()
    {
        bool isMoving = movement.IsMovingTowardsTarget();
        animator.SetBool("IsMoving", isMoving);
    }

    public bool CanSeePlayer()
    {
        if (player == null) return false;

        Vector3 dirToPlayer = player.position - transform.position;
        float distance = dirToPlayer.magnitude;

        if (distance > viewDistance) return false;

        Vector3 dirNormalized = dirToPlayer.normalized;
        float angle = Vector3.Angle(transform.forward, dirNormalized);

        if (angle > viewAngle * 0.5f) return false;

        if (Physics.Raycast(transform.position + Vector3.up,
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
        if (currentState != null && currentState.GetType() == newState.GetType())
            return;

        currentState?.Exit(this);
        currentState = newState;

        if (enemyAudio != null)
        {
            if (newState is ChaseState)
                enemyAudio.PlayChaseLoop();
            else
                enemyAudio.StopChaseLoop();
        }

        bool isCombatState =
            newState is ChaseState ||
            newState is AttackState ||
            newState is StunnedState;

        if (isCombatState)
            CombatMusicController.Instance?.RegisterEnemyCombat(this);
        else if (newState is PatrolState || newState is DeadState)
            CombatMusicController.Instance?.UnregisterEnemyCombat(this);

        currentState.Enter(this);
    }
}
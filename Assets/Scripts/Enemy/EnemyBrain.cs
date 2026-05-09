using UnityEngine;

public class EnemyBrain : MonoBehaviour
{
    [Header("Refs")]
    public Transform player;

    public EnemyMovement movement;
    public EnemyStun stun;
    public EnemyAttack attack;
    public Animator animator;

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
        attack = GetComponent<EnemyAttack>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        ChangeState(new PatrolState());
    }

    private void Update()
    {
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
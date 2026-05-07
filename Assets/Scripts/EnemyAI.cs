using UnityEngine;

public class EnemyAI : MonoBehaviour, IStunnable, IDamageable
{
    [Header("Referencias")]
    [SerializeField] private Transform player;

    [Header("Movimiento")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float chaseSpeed = 7f;

    [Header("Rotación")]
    [SerializeField] private float patrolRotationSpeed = 2.5f;
    [SerializeField] private float chaseRotationSpeed = 15f;

    [Header("Detección")]
    [SerializeField] private float detectionRange = 8f;
    [SerializeField] private float stopDistance = 1.8f;

    [Header("Patrulla")]
    [SerializeField] private float patrolRadius = 6f;
    [SerializeField] private float waitTime = 2f;

    [Header("Anti-Traspaso")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float obstacleCheckDistance = 1.2f;

    [Header("Vida")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;

    [Header("Stun")]
    [SerializeField] private float stunCooldown = 2f;
    [SerializeField] private float stunKnockback = 0.5f;

    private Rigidbody rb;

    private Vector3 startPoint;
    private Vector3 targetPoint;

    private float waitCounter;
    private float groundY;

    private bool isStunned = false;
    private float stunTimer;
    private float lastStunTime;

    // =========================================================
    // UNITY
    // =========================================================

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.freezeRotation = true;

        currentHealth = maxHealth;

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null)
                player = obj.transform;
        }

        startPoint = transform.position;
        groundY = transform.position.y;

        SetNewRandomPoint();

        if (obstacleLayer.value == 0)
        {
            obstacleLayer = LayerMask.GetMask("Default", "Wall");
        }
    }

    private void FixedUpdate()
    {
        if (player == null)
            return;

        if (isStunned)
        {
            stunTimer -= Time.fixedDeltaTime;

            if (stunTimer <= 0f)
                isStunned = false;

            return;
        }

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange && HasLineOfSightToPlayer())
        {
            ChasePlayer(distance);
        }
        else
        {
            Patrol();
        }
    }

    // =========================================================
    // VIDA
    // =========================================================

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        Debug.Log($"{gameObject.name} HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} murió");
        Destroy(gameObject);
    }

    // =========================================================
    // STUN
    // =========================================================

    public void Stun(float duration)
    {
        if (Time.time < lastStunTime + stunCooldown)
            return;

        lastStunTime = Time.time;

        isStunned = true;
        stunTimer = duration;

        rb.MovePosition(transform.position - transform.forward * stunKnockback);

        Debug.Log($"{gameObject.name} stunned");
    }

    // =========================================================
    // IA (sin cambios importantes)
    // =========================================================

    private void Patrol()
    {
        if (CanMoveTo(targetPoint))
        {
            MoveTo(targetPoint, patrolRotationSpeed, speed);
        }
        else
        {
            SetNewRandomPoint();
        }

        if (Vector3.Distance(transform.position, targetPoint) < 0.5f)
        {
            waitCounter += Time.fixedDeltaTime;

            if (waitCounter >= waitTime)
            {
                SetNewRandomPoint();
                waitCounter = 0f;
            }
        }
    }

    private void ChasePlayer(float distance)
    {
        if (distance > stopDistance)
        {
            if (CanMoveTo(player.position))
            {
                MoveTo(player.position, chaseRotationSpeed, chaseSpeed);
            }
        }
    }

    private void MoveTo(Vector3 target, float rotationSpeed, float moveSpeed)
    {
        Vector3 fixedTarget = new Vector3(target.x, groundY, target.z);
        Vector3 direction = (fixedTarget - transform.position).normalized;

        Vector3 newPosition =
            transform.position +
            direction * moveSpeed * Time.fixedDeltaTime;

        newPosition.y = groundY;

        rb.MovePosition(newPosition);

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.fixedDeltaTime * rotationSpeed
            );
        }
    }

    // =========================================================
    // HELPERS
    // =========================================================

    private void SetNewRandomPoint()
    {
        const int maxAttempts = 50;
        int attempts = 0;

        while (attempts < maxAttempts)
        {
            Vector2 randomCircle = Random.insideUnitCircle * patrolRadius;

            Vector3 candidatePoint = new Vector3(
                startPoint.x + randomCircle.x,
                groundY,
                startPoint.z + randomCircle.y
            );

            if (!Physics.CheckSphere(candidatePoint, 0.3f, obstacleLayer))
            {
                targetPoint = candidatePoint;
                return;
            }

            attempts++;
        }

        targetPoint = startPoint;
    }

    private bool HasLineOfSightToPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        return !Physics.Raycast(transform.position, direction, distance, obstacleLayer);
    }

    private bool CanMoveTo(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, target);

        return !Physics.Raycast(transform.position, direction, distance, obstacleLayer);
    }
}
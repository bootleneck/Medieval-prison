using UnityEngine;

public class EnemyAI : MonoBehaviour
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

    [Header("Anti-traspaso y altura")]
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float obstacleCheckDistance = 1.2f;

    private Rigidbody rb;
    private Vector3 startPoint;
    private Vector3 targetPoint;
    private float waitCounter;
    private bool isChasing = false;
    private float groundY;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.freezeRotation = true;

        if (player == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("Player");
            if (obj != null) player = obj.transform;
        }

        startPoint = transform.position;
        groundY = transform.position.y;

        SetNewRandomPoint();

        if (obstacleLayer.value == 0)
            obstacleLayer = LayerMask.GetMask("Default", "Wall");
    }

    void FixedUpdate()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= detectionRange && HasLineOfSightToPlayer())
        {
            isChasing = true;
            ChasePlayer(distance);
        }
        else
        {
            isChasing = false;
            Patrol();
        }
    }

    // PATRULLA 
    void Patrol()
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

    // PERSECUCIÓN 
    void ChasePlayer(float distance)
    {
        if (distance > stopDistance)
        {
            if (CanMoveTo(player.position))
            {
                MoveTo(player.position, chaseRotationSpeed, chaseSpeed);
            }
            else
            {
                isChasing = false;
            }
        }
    }

    // MOVIMIENTO 
    void MoveTo(Vector3 target, float currentRotationSpeed, float currentMoveSpeed)
    {
        Vector3 fixedTarget = new Vector3(target.x, groundY, target.z);
        Vector3 direction = (fixedTarget - transform.position).normalized;

        Vector3 newPosition = transform.position + direction * currentMoveSpeed * Time.fixedDeltaTime;
        newPosition.y = groundY;

        rb.MovePosition(newPosition);

        if (direction != Vector3.zero)
        {
            Vector3 flatDirection = new Vector3(direction.x, 0f, direction.z).normalized;
            if (flatDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * currentRotationSpeed);
            }
        }
    }

    // PUNTO ALEATORIO
    void SetNewRandomPoint()
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

            
            if (!Physics.CheckSphere(candidatePoint, 0.3f, obstacleLayer, QueryTriggerInteraction.Ignore))
            {
                targetPoint = candidatePoint;
                return;
            }

            attempts++;
        }
        targetPoint = startPoint;
            }

    bool HasLineOfSightToPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);
        return !Physics.Raycast(transform.position, direction, distance, obstacleLayer);
    }

    bool CanMoveTo(Vector3 target)
    {
        Vector3 fixedTarget = new Vector3(target.x, groundY, target.z);
        Vector3 direction = (fixedTarget - transform.position).normalized;
        float distanceToTarget = Vector3.Distance(transform.position, fixedTarget);

        return !Physics.Raycast(transform.position, direction, Mathf.Min(obstacleCheckDistance, distanceToTarget), obstacleLayer);
    }
}

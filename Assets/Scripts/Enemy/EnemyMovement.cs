using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Speeds")]
    [SerializeField] private float patrolSpeed = 2.5f;
    [SerializeField] private float chaseSpeed = 5.5f;

    [Header("NavMesh Settings")]
    [SerializeField] private float patrolAcceleration = 8f;
    [SerializeField] private float chaseAcceleration = 20f;

    [SerializeField] private float patrolAngularSpeed = 120f;
    [SerializeField] private float chaseAngularSpeed = 360f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        SetPatrolMode();
    }

    // =========================================================
    // MOVIMIENTO
    // =========================================================

    public void MoveTo(Vector3 position)
    {
        agent.isStopped = false;
        agent.SetDestination(position);
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public void Resume()
    {
        agent.isStopped = false;
    }

    // =========================================================
    // MODOS
    // =========================================================

    public void SetPatrolSpeed()
    {
        agent.speed = patrolSpeed;
        SetPatrolMode();
    }

    public void SetChaseSpeed()
    {
        agent.speed = chaseSpeed;
        SetChaseMode();
    }

    private void SetPatrolMode()
    {
        agent.acceleration = patrolAcceleration;
        agent.angularSpeed = patrolAngularSpeed;
    }

    private void SetChaseMode()
    {
        agent.acceleration = chaseAcceleration;
        agent.angularSpeed = chaseAngularSpeed;
    }

    // =========================================================
    // FSM UTIL
    // =========================================================

    public bool HasArrived(float threshold = 0.5f)
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance >
            agent.stoppingDistance + threshold)
            return false;

        if (agent.velocity.sqrMagnitude > 0.05f)
            return false;

        return true;
    }

    // =========================================================
    // ANIMACIÓN (NO USAR VELOCIDAD)
    // =========================================================

    public bool IsMovingTowardsTarget()
    {
        if (agent.pathPending)
            return true;

        if (agent.remainingDistance > agent.stoppingDistance)
            return true;

        return false;
    }
}
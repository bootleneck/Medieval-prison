using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    private NavMeshAgent agent;

    [Header("Speeds")]
    [SerializeField] private float patrolSpeed = 2.5f;
    [SerializeField] private float chaseSpeed = 5.5f;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.speed = patrolSpeed;
    }

    public void MoveTo(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void Stop()
    {
        agent.isStopped = true;
        agent.ResetPath();
    }

    public void Resume()
    {
        agent.isStopped = false;
    }

    public void SetPatrolSpeed()
    {
        agent.speed = patrolSpeed;
        agent.acceleration = 8f;
    }

    public void SetChaseSpeed()
    {
        agent.speed = chaseSpeed;
        agent.acceleration = 20f;
    }

    public bool HasArrived(float threshold = 0.5f)
    {
        if (agent.pathPending) return false;
        if (agent.remainingDistance > agent.stoppingDistance + threshold) return false;
        if (agent.velocity.sqrMagnitude > 0.05f) return false;

        return true;
    }
}
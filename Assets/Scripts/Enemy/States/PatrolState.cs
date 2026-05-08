using UnityEngine;
using UnityEngine.AI;

public class PatrolState : EnemyState
{
    private Vector3 targetPoint;
    private Vector3 lastDirection;

    private float waitTimer;
    private float waitTime;
    private bool waiting;

    public override void Enter(EnemyBrain brain)
    {
        brain.movement.SetPatrolSpeed();

        waiting = false;
        waitTimer = 0f;

        waitTime = Random.Range(3f, 6f);

        PickNewPoint(brain);
    }

    public override void Update(EnemyBrain brain)
    {
        // 🔥 visión real
        if (brain.CanSeePlayer())
        {
            brain.ChangeState(new ChaseState());
            return;
        }

        if (!waiting)
        {
            brain.movement.MoveTo(targetPoint);

            if (brain.movement.HasArrived(0.8f))
            {
                waiting = true;
                waitTimer = 0f;
                waitTime = Random.Range(3f, 6f);
            }
        }
        else
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitTime)
            {
                PickNewPoint(brain);
                waiting = false;
            }
        }
    }

    public override void Exit(EnemyBrain brain) { }

    void PickNewPoint(EnemyBrain brain)
    {
        var agent = brain.GetComponent<NavMeshAgent>();

        Vector3 baseDir =
            lastDirection == Vector3.zero
            ? brain.transform.forward
            : lastDirection;

        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDir =
                Quaternion.Euler(0, Random.Range(-110f, 110f), 0) * baseDir;

            float distance = Random.Range(3f, 7f);

            Vector3 candidate =
                brain.transform.position + randomDir.normalized * distance;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 3f, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();

                if (agent.CalculatePath(hit.position, path) &&
                    path.status == NavMeshPathStatus.PathComplete)
                {
                    targetPoint = hit.position;
                    lastDirection =
                        (targetPoint - brain.transform.position).normalized;
                    return;
                }
            }
        }

        targetPoint = brain.transform.position;
    }
}
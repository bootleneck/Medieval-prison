using UnityEngine;

public class ChaseState : EnemyState
{
    public override void Enter(EnemyBrain brain)
    {
        brain.movement.SetChaseSpeed();
    }

    public override void Update(EnemyBrain brain)
    {
        if (!brain.CanSeePlayer())
        {
            brain.ChangeState(new PatrolState());
            return;
        }

        float dist = Vector3.Distance(
            brain.transform.position,
            brain.player.position
        );

        if (dist <= brain.attackRange)
        {
            brain.ChangeState(new AttackState());
            return;
        }

        brain.movement.MoveTo(brain.player.position);
    }

    public override void Exit(EnemyBrain brain) { }
}
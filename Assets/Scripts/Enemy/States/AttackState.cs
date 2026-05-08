using UnityEngine;

public class AttackState : EnemyState
{
    public override void Enter(EnemyBrain brain)
    {
        brain.movement.Stop();
    }

    public override void Update(EnemyBrain brain)
    {
        float dist = Vector3.Distance(brain.transform.position, brain.player.position);

        if (dist > brain.attackRange)
        {
            brain.ChangeState(new ChaseState());
            return;
        }

        // daño / animación aquí
    }

    public override void Exit(EnemyBrain brain) { }
}
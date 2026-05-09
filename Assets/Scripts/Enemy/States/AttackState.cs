using UnityEngine;

public class AttackState : EnemyState
{
    public override void Enter(EnemyBrain brain)
    {
        brain.movement.Stop();
    }

    public override void Update(EnemyBrain brain)
    {
        if (brain.player == null)
            return;

        float dist = Vector3.Distance(
            brain.transform.position,
            brain.player.position
        );

        if (dist > brain.attackRange)
        {
            brain.ChangeState(new ChaseState());
            return;
        }

        FacePlayer(brain);

        if (brain.attack.CanAttack)
        {
            brain.attack.StartAttack();
        }
    }

    private void FacePlayer(EnemyBrain brain)
    {
        Vector3 dir =
            (brain.player.position - brain.transform.position);

        dir.y = 0;

        Quaternion rot = Quaternion.LookRotation(dir);

        brain.transform.rotation = Quaternion.Slerp(
            brain.transform.rotation,
            rot,
            Time.deltaTime * 10f
        );
    }

    public override void Exit(EnemyBrain brain) { }
}
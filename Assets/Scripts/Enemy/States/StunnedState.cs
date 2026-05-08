using UnityEngine;

public class StunnedState : EnemyState
{
    public override void Enter(EnemyBrain brain)
    {
        brain.movement.Stop();
    }

    public override void Update(EnemyBrain brain)
    {
        if (!brain.stun.IsStunned)
        {
            brain.movement.Resume();
            brain.ChangeState(new PatrolState());
        }
    }

    public override void Exit(EnemyBrain brain)
    {
        brain.movement.Resume();
    }
}
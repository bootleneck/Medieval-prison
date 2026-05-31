using UnityEngine;

public class StunnedState : EnemyState
{
    public override void Enter(EnemyBrain brain)
    {
        brain.movement.Stop();
        // Opcional: reproducir animación de stun
        // brain.animator.SetTrigger("Stun");
    }

    public override void Update(EnemyBrain brain)
    {
        // Solo salimos cuando ya no está stunned
        if (!brain.stun.IsStunned)
        {
            brain.movement.Resume();

            // ← NUEVA LÓGICA: Decidir correctamente el siguiente estado
            if (brain.CanSeePlayer())
                brain.ChangeState(new ChaseState());
            else
                brain.ChangeState(new PatrolState());
        }
    }

    public override void Exit(EnemyBrain brain)
    {
        brain.movement.Resume();
    }
}
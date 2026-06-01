using UnityEngine;

public class AttackState : EnemyState
{
    private EnemyBrain brain;
    private float meleeRange;
    private float rangedRange = 10f;

    public override void Enter(EnemyBrain brain)
    {
        this.brain = brain;
        brain.movement.Stop();

        if (brain.meleeAttack != null)
            meleeRange = brain.meleeAttack.AttackRange;
    }

    public override void Update(EnemyBrain brain)
    {
        if (brain.player == null) return;

        float dist = Vector3.Distance(brain.transform.position, brain.player.position);
        FacePlayer();

        // -----------------------
        // PRIORIDAD RANGED
        // -----------------------
        if (brain.TryGetComponent<EnemyRangedAttack>(out var rangedAttack))
        {
            if (dist <= rangedRange && rangedAttack.CanAttack)
            {
                // Dispara ranged y detiene el movimiento
                rangedAttack.StartRangedAttack();
                brain.movement.Stop();
                return; // no hacer melee en el mismo frame
            }
        }

        // -----------------------
        // ATAQUE MELEE
        // -----------------------
        if (brain.meleeAttack != null && dist <= meleeRange && brain.meleeAttack.CanAttack)
        {
            brain.meleeAttack.StartAttack();
            return;
        }

        // -----------------------
        // FUERA DE RANGED → CHASE
        // -----------------------
        if (dist > rangedRange)
        {
            brain.ChangeState(new ChaseState());
        }
        else
        {
            // Persigue al jugador mientras el ataque está en cooldown
            brain.movement.MoveTo(brain.player.position);
        }
    }

    private void FacePlayer()
    {
        Vector3 dir = brain.player.position - brain.transform.position;
        dir.y = 0;
        if (dir == Vector3.zero) return;

        Quaternion rot = Quaternion.LookRotation(dir);
        brain.transform.rotation = Quaternion.Slerp(
            brain.transform.rotation,
            rot,
            Time.deltaTime * 10f
        );
    }

    public override void Exit(EnemyBrain brain)
    {
        brain.movement.Resume();
    }
}
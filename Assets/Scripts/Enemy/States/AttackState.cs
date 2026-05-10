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
        meleeRange = brain.attack.AttackRange;
    }

    public override void Update(EnemyBrain brain)
    {
        if (brain.player == null) return;

        float dist = Vector3.Distance(brain.transform.position, brain.player.position);
        FacePlayer();

        // -----------------------
        // PRIORIDAD RANGED
        // -----------------------
        if (dist <= rangedRange)
        {
            if (brain.TryGetComponent<EnemyRangedAttack>(out var rangedAttack) && rangedAttack.CanAttack)
            {
                // Dispara ranged y detiene el movimiento
                rangedAttack.StartRangedAttack();
                brain.movement.Stop();
            }
            else
            {
                // Si ranged está en cooldown
                if (dist <= meleeRange)
                {
                    // Ejecuta melee si está en rango
                    if (brain.attack.CanAttack)
                        brain.attack.StartAttack();
                }
                else
                {
                    // Persigue al jugador mientras el ranged recarga
                    brain.movement.MoveTo(brain.player.position);
                }
            }
        }
        // -----------------------
        // FUERA DE RANGED → CHASE
        // -----------------------
        else
        {
            brain.ChangeState(new ChaseState());
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
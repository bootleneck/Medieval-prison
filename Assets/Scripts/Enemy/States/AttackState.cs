using UnityEngine;

public class AttackState : EnemyState
{
    private EnemyBrain brain;
    private float meleeRange;
    private float rangedRange;
    private bool mustMelee = false; // ← tras ranged, fuerza melee

    public override void Enter(EnemyBrain brain)
    {
        this.brain = brain;
        brain.movement.Stop();

        if (brain.meleeAttack != null)
            meleeRange = brain.meleeAttack.AttackRange;

        rangedRange = brain.rangedAttackRange;
    }

    public override void Update(EnemyBrain brain)
    {
        if (brain.player == null) return;

        float dist = Vector3.Distance(brain.transform.position, brain.player.position);
        FacePlayer();

        // -----------------------
        // FUERA DE RANGO → CHASE
        // -----------------------
        if (dist > rangedRange && !mustMelee)
        {
            brain.ChangeState(new ChaseState());
            return;
        }

        // -----------------------
        // MODO PERSECUCIÓN FORZADA (después de ranged)
        // -----------------------
        if (mustMelee)
        {
            if (dist <= meleeRange)
            {
                // llegó cerca → hace melee y resetea el flag
                if (brain.meleeAttack != null && brain.meleeAttack.CanAttack)
                {
                    brain.meleeAttack.StartAttack();
                    mustMelee = false;
                }
            }
            else
            {
                // persigue hasta llegar a rango melee
                brain.movement.MoveTo(brain.player.position);
            }
            return;
        }

        // -----------------------
        // LEJOS: RANGED o acercarse
        // -----------------------
        if (dist > meleeRange)
        {
            if (brain.TryGetComponent<EnemyRangedAttack>(out var rangedAttack))
            {
                if (rangedAttack.CanAttack)
                {
                    rangedAttack.StartRangedAttack();
                    brain.movement.Stop();
                    mustMelee = true; // ← próximo ataque será melee
                }
                else
                {
                    brain.movement.Stop();
                }
            }
            else
            {
                brain.movement.MoveTo(brain.player.position);
            }
            return;
        }

        // -----------------------
        // CERCA: solo MELEE
        // -----------------------
        if (brain.meleeAttack != null && brain.meleeAttack.CanAttack)
        {
            brain.meleeAttack.StartAttack();
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
        mustMelee = false; // limpiamos el flag al salir del estado
        brain.movement.Resume();
    }
}
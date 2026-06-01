using UnityEngine;

public class DeadState : EnemyState
{
    public override void Enter(EnemyBrain enemy)
    {
        Debug.Log($"[DeadState] {enemy.gameObject.name} entró en estado de muerte.");

        EnemyAudio enemyAudio = enemy.GetComponent<EnemyAudio>();
        enemyAudio?.PlayDeath();

        // 🔒 cortar lógica de combate
        enemy.SetAttacking(false);

        // ❌ desactivar sistemas
        if (enemy.movement != null)
            enemy.movement.enabled = false;

        if (enemy.meleeAttack != null)
            enemy.meleeAttack.enabled = false;

        if (enemy.rangedAttack != null)
            enemy.rangedAttack.enabled = false;

        if (enemy.stun != null)
            enemy.stun.enabled = false;

        // 🎬 animación de muerte
        if (enemy.animator != null)
        {
            enemy.animator.SetBool("IsMoving", false);
            enemy.animator.SetTrigger("OnDead");
        }

        // 🧱 collider off
        Collider col = enemy.GetComponent<Collider>();
        if (col != null)
            col.enabled = false;
    }

    public override void Update(EnemyBrain enemy) { }

    public override void Exit(EnemyBrain enemy) { }
}
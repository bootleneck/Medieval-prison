using UnityEngine;

public class DeadState : EnemyState
{
    public override void Enter(EnemyBrain enemy)
    {
        Debug.Log($"[DeadState] {enemy.gameObject.name} entró en estado de muerte.");

        EnemyAudio enemyAudio = enemy.GetComponent<EnemyAudio>();

        if (enemyAudio != null)
        {
            enemyAudio.PlayDeath();
        }

        if (enemy.movement != null) enemy.movement.enabled = false;
        if (enemy.attack != null) enemy.attack.enabled = false;
        if (enemy.stun != null) enemy.stun.enabled = false;

        if (enemy.animator != null)
        {
            enemy.animator.SetBool("IsMoving", false);
            enemy.animator.SetTrigger("OnDead");
        }

        Collider col = enemy.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = false;
        }
    }

    public override void Update(EnemyBrain enemy) { }

    public override void Exit(EnemyBrain enemy) { }
}
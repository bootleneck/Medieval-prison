using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask hitLayers;

    [Header("Attack Range")]
    [SerializeField] private float attackRange = 2f;
    public float AttackRange => attackRange;

    [Header("Box Settings")]
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(1f, 1f, 2f);

    [Header("Hit Timing")]
    [SerializeField] private float windupTime = 0.4f;
    [SerializeField] private float recoveryTime = 0.3f;

    [Header("References")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private Animator animator;

    private EnemyAudio enemyAudio;

    private float lastAttackTime;
    private bool isAttacking;

    public bool CanAttack => Time.time >= lastAttackTime + attackCooldown && !isAttacking;

    private void Awake()
    {
        enemyAudio = GetComponent<EnemyAudio>();

        if (attackPoint == null)
            Debug.LogWarning("[EnemyMeleeAttack] No se asignó attackPoint");
    }

    public void StartAttack()
    {
        var brain = GetComponent<EnemyBrain>();

        if (!CanAttack || brain.IsAttacking)
            return;

        StartCoroutine(AttackRoutine(brain));
    }

    private IEnumerator AttackRoutine(EnemyBrain brain)
    {
        brain.SetAttacking(true);

        isAttacking = true;
        lastAttackTime = Time.time;

        animator.SetTrigger("Attack");
        enemyAudio?.PlayMeleeAttack();

        // WINDUP (telegraph)
        yield return new WaitForSeconds(windupTime);

        // 🔥 UN SOLO HIT
        DealDamageOnce();

        // RECOVERY
        yield return new WaitForSeconds(recoveryTime);

        isAttacking = false;
        brain.SetAttacking(false);
    }

    private void DealDamageOnce()
    {
        if (attackPoint == null) return;

        Collider[] hits = Physics.OverlapBox(
            attackPoint.position,
            boxHalfExtents,
            attackPoint.rotation,
            hitLayers
        );

        HashSet<IDamageable> hitTargets = new HashSet<IDamageable>();

        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponentInParent<IDamageable>();

            if (dmg != null && !hitTargets.Contains(dmg))
            {
                hitTargets.Add(dmg);
                dmg.TakeDamage(attackDamage);

                if (dmg is Component comp)
                {
                    Debug.Log($"💥 {comp.gameObject.name} recibió {attackDamage} de daño");
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(
            attackPoint.position,
            attackPoint.rotation,
            Vector3.one
        );

        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2);
    }
}
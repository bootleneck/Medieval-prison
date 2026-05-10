using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;
    [SerializeField] private LayerMask hitLayers;

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;

    private float lastAttackTime;

    public bool CanAttack => Time.time >= lastAttackTime + attackCooldown;

    public void StartAttack()
    {
        if (!CanAttack) return;

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");

        Debug.Log("⚔ Enemy inicia ataque");
    }

    // Animation Event
    public void DealDamage()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("❌ Falta asignar Attack Point");
            return;
        }

        Debug.Log("⚔ Hit frame ejecutado");

        Collider[] hits = Physics.OverlapSphere(
            attackPoint.position,
            attackRange,
            hitLayers
        );

        bool hitSomeone = false;

        HashSet<IDamageable> damagedTargets = new();

        foreach (var hit in hits)
        {
            IDamageable dmg =
                hit.GetComponentInParent<IDamageable>();

            if (dmg != null && !damagedTargets.Contains(dmg))
            {
                damagedTargets.Add(dmg);

                dmg.TakeDamage(attackDamage);

                var dmgGO = (dmg as Component).gameObject;

                Debug.Log($"💥 {dmgGO.name} recibió {attackDamage} de daño");

                hitSomeone = true;
            }
        }

        if (!hitSomeone)
        {
            Debug.Log("❌ No hay nadie en rango");
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            attackPoint.position,
            attackRange
        );
    }
}
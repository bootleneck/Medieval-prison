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
    public float AttackRange => attackRange; // <--- propiedad pública

    [Header("Box Settings")]
    [SerializeField] private Vector3 boxHalfExtents = new Vector3(1f, 1f, 2f); // x=ancho, y=alto, z=profundidad

    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private Transform attackPoint;

    private float lastAttackTime;

    public bool CanAttack => Time.time >= lastAttackTime + attackCooldown;

    // =========================================================
    // Inicia el ataque
    // =========================================================
    public void StartAttack()
    {
        if (!CanAttack) return;

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");

        Debug.Log("⚔ Enemy inicia ataque melee");
    }

    // =========================================================
    // Animation Event
    // =========================================================
    public void DealDamage()
    {
        if (attackPoint == null)
        {
            Debug.LogWarning("❌ Falta asignar Attack Point");
            return;
        }

        Debug.Log("⚔ Hit frame ejecutado");

        // Detectamos colliders en el box
        Collider[] hits = Physics.OverlapBox(
            attackPoint.position,
            boxHalfExtents,
            attackPoint.rotation,
            hitLayers
        );

        bool hitSomeone = false;
        HashSet<IDamageable> damagedTargets = new();

        foreach (var hit in hits)
        {
            IDamageable dmg = hit.GetComponentInParent<IDamageable>();
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

    // =========================================================
    // Gizmos para visualizar el box
    // =========================================================
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.red;
        Gizmos.matrix = Matrix4x4.TRS(attackPoint.position, attackPoint.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, boxHalfExtents * 2); // OverlapBox usa half extents, Gizmos requiere tamaño completo
    }
}
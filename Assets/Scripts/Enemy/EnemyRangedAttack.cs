using System.Collections;
using UnityEngine;

public class EnemyRangedAttack : MonoBehaviour
{
    [Header("Ranged Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private GameObject acidPrefab;  // prefab AcidTrail
   // [SerializeField] private float projectileSpeed = 10f;

    [Header("References")]
    [SerializeField] private Animator animator;

    private float lastAttackTime;

    public bool CanAttack => Time.time >= lastAttackTime + attackCooldown;

    // =========================================================
    // Inicio de ataque (Animation Trigger)
    // =========================================================
    public void StartRangedAttack()
    {
        if (!CanAttack) return;

        lastAttackTime = Time.time;
        animator.SetTrigger("RangedAttack");
        Debug.Log("🏹 Enemy dispara ataque ranged");
    }

    // =========================================================
    // Animation Event: disparo de ácido
    // =========================================================
    public void ShootAcid()
    {
        if (attackPoint == null || acidPrefab == null) return;

        // Instanciamos prefab
        GameObject acid = Instantiate(acidPrefab, attackPoint.position, attackPoint.rotation);

        // Configuramos el proyectil
        if (acid.TryGetComponent<AcidProjectile>(out var proj))
        {
         //   proj.speed = projectileSpeed;
            proj.damage = damage;
            proj.lifetime = 3f; // opcional: ajustar duración
        }

        Debug.Log("💨 Acid disparado");
    }

    // =========================================================
    // Visualización en Scene (Gizmos)
    // =========================================================
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null) return;

        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(attackPoint.position, attackPoint.position + attackPoint.forward * maxDistance);
    }
}
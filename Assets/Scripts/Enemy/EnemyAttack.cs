using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Damage Settings")]
    [SerializeField] private int attackDamage = 20;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float attackCooldown = 1.5f;

    [Header("References")]
    [SerializeField] private Animator animator;

    private float lastAttackTime;

    public bool CanAttack => Time.time >= lastAttackTime + attackCooldown;

    public void StartAttack()
    {
        if (!CanAttack) return;

        lastAttackTime = Time.time;
        animator.SetTrigger("Attack");

        Debug.Log("⚔️ Enemy inicia ataque");
    }

    public void DealDamage()
    {
        Debug.Log("⚔️ Hit frame ejecutado");

        Collider[] hits = Physics.OverlapSphere(transform.position, attackRange);

        bool hitSomeone = false;

        foreach (var hit in hits)
        {
            if (hit.transform.root.TryGetComponent<IDamageable>(out var dmg))
            {
                dmg.TakeDamage(attackDamage);

                // Corregido: acceder al GameObject real
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
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
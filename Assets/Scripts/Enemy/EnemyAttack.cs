using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [Header("Daño")]
    [SerializeField] private int attackDamage = 20;

    [Header("Cooldown")]
    [SerializeField] private float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;

    private IDamageable playerDamageable;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDamageable = other.GetComponent<IDamageable>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerDamageable = null;
        }
    }

    private void Update()
    {
        if (playerDamageable == null) return;

        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        if (playerDamageable != null)
        {
            playerDamageable.TakeDamage(attackDamage);
            Debug.Log($"{gameObject.name} atacó al jugador por {attackDamage} daño");
        }
    }
}
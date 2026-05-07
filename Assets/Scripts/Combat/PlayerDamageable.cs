using UnityEngine;

public class PlayerDamageable : MonoBehaviour, IDamageable
{
    [Header("Vida del jugador")]
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"Jugador recibió {damage} de daño. HP: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Jugador muerto");
        // Aquí puedes llamar animaciones, reinicio de nivel, Game Over, etc.
    }
}
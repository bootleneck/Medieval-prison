using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    public bool IsDead { get; private set; }

    // Eventos
    public event Action<int> OnDamageTaken;
    public event Action<int> OnHealed;      // Para UI futura
    public event Action OnDeath;            // ← Este era el que faltaba

    private void Awake()
    {
        CurrentHealth = maxHealth;
        Debug.Log($"[Health] Vida inicial: {CurrentHealth}/{MaxHealth}");
    }

    public void TakeDamage(int damage)
    {
        if (IsDead) return;

        CurrentHealth -= damage;
        if (CurrentHealth < 0) CurrentHealth = 0;

        Debug.Log($"[Health] Recibió {damage} daño → Vida actual: {CurrentHealth}/{MaxHealth}");

        OnDamageTaken?.Invoke(damage);

        if (CurrentHealth <= 0)
            Die();
    }

    public void Heal(int amount)
    {
        if (IsDead || amount <= 0) return;

        int healthBefore = CurrentHealth;
        CurrentHealth += amount;
        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;

        int realHeal = CurrentHealth - healthBefore;

        Debug.Log($"[Health] ¡CURADO +{realHeal} HP! Vida ahora: {CurrentHealth}/{MaxHealth}");

        OnHealed?.Invoke(realHeal);
    }

    private void Die()
    {
        if (IsDead) return;

        IsDead = true;
        Debug.Log($"[Health] {gameObject.name} ha muerto");
        OnDeath?.Invoke();
    }
}
using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [Header("Health")]
    [SerializeField] private int maxHealth = 100;

    public int CurrentHealth { get; private set; }
    public int MaxHealth => maxHealth;

    public bool IsDead { get; private set; }

    // eventos
    public event Action<int> OnDamageTaken;
    public event Action OnDeath;

    private void Awake()
    {
        CurrentHealth = maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;

        CurrentHealth -= damage;

        if (CurrentHealth < 0)
            CurrentHealth = 0;

        OnDamageTaken?.Invoke(damage);

        Debug.Log($"{gameObject.name} HP: {CurrentHealth}");

        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        OnDeath?.Invoke();

        Debug.Log($"{gameObject.name} murió");
    }

    public void Heal(int amount)
    {
        if (IsDead)
            return;

        CurrentHealth += amount;

        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;
    }
}
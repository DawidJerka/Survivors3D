using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float maxHealth = 100f;

    private float maxHealthMultiplier = 1f;

    public float CurrentHealth { get; private set; }

    public float MaxHealth =>
        maxHealth * maxHealthMultiplier;

    public bool IsDead { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action OnDied;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead || damage <= 0f)
            return;

        CurrentHealth = Mathf.Max(
            CurrentHealth - damage,
            0f
        );

        OnHealthChanged?.Invoke(
            CurrentHealth,
            MaxHealth
        );

        if (CurrentHealth <= 0f)
        {
            Die();
        }
    }

    public void SetMaxHealthMultiplier(float multiplier)
    {
        multiplier = Mathf.Max(multiplier, 0.01f);

        float previousMaxHealth = MaxHealth;

        maxHealthMultiplier = multiplier;

        float newMaxHealth = MaxHealth;

        if (newMaxHealth > previousMaxHealth)
        {
            CurrentHealth +=
                newMaxHealth - previousMaxHealth;
        }
        else
        {
            CurrentHealth =
                Mathf.Min(CurrentHealth, newMaxHealth);
        }

        OnHealthChanged?.Invoke(
            CurrentHealth,
            MaxHealth
        );
    }

    public void SetBaseMaxHealth(
    float value,
    bool healToFull = true)
    {
        maxHealth = Mathf.Max(1f, value);

        if (healToFull)
        {
            CurrentHealth = MaxHealth;
        }
        else
        {
            CurrentHealth =
                Mathf.Min(CurrentHealth, MaxHealth);
        }

        OnHealthChanged?.Invoke(
            CurrentHealth,
            MaxHealth
        );
    }

    private void Die()
    {
        if (IsDead)
            return;

        IsDead = true;

        OnDied?.Invoke();
    }
}
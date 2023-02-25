using System;
using UnityEngine;

public class Health
{
    private readonly Character character;
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public bool IsDead { get; private set; }

    public event Action<(float currentHealth, float maxHealth)> HealthChanged, GotDamage;
    public event Action Died;

    public Health(Character character, float maxValue) : this(character, maxValue, maxValue) { }

    public Health(Character character, float maxValue, float currentValue)
    {
        this.character = character;
        CurrentHealth = currentValue;
        MaxHealth = maxValue;
        IsDead = false;
    }

    public void SetCurrentHealth(float health)
    {
        CurrentHealth = health;
    }

    public void SetIsDead(bool value)
    {
        IsDead = value;
    }

    public void ChangeHealth(float value, bool effectDead = false)
    {
        if (IsDead && !effectDead)
        {
            return;
        }
        CurrentHealth = Mathf.Clamp(CurrentHealth + value, 0, MaxHealth);
        if (value < 0)
        {
            GotDamage?.Invoke((CurrentHealth, MaxHealth));
        }
        SavesManager.Instance.UpdateCharacter(character);
        HealthChanged?.Invoke((CurrentHealth, MaxHealth));
        if (CurrentHealth == 0)
        {
            Death();
        }
    }

    public void AddHealthBonus(float value)
    {
        MaxHealth += value;
        CurrentHealth += value;
        HealthChanged?.Invoke((CurrentHealth, MaxHealth));
    }

    public void Death()
    {
        IsDead = true;
        Died?.Invoke();
    }
}

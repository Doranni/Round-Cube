using System;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(StatsManager))]
public class Health : MonoBehaviour
{
    public float CurrentHealth { get; private set; }
    public float MaxHealth { get; private set; }
    public bool IsDead { get; private set; }

    private StatsManager statsManager;

    public event Action<(float currentHealth, float maxHealth)> OnChangeHealth, OnGetDamage, 
        OnChangeMaxHealth;
    public event Action OnDeath;

    private void Start()
    {
        statsManager = GetComponent<StatsManager>();
        CurrentHealth = MaxHealth = statsManager.Stats[Stat.StatId.health].BaseValue;
        IsDead = false;

        statsManager.Stats[Stat.StatId.health].OnAddBonus += Health_CalcBonus;
        statsManager.Stats[Stat.StatId.health].OnRemoveBonus += x => Health_CalcBonus(-x);
    }

    private void Health_CalcBonus(int value)
    {
        Debug.Log("Health_CalcBonus");
        ChangeMaxHealth(value);
        ChangeHealth(value);
        Debug.Log($"current health - {CurrentHealth}, max health - {MaxHealth}");
    }

    private void ChangeMaxHealth(float value)
    {
        MaxHealth = Mathf.Clamp(MaxHealth + value, 0, MaxHealth + value);
        OnChangeMaxHealth?.Invoke((CurrentHealth, MaxHealth));
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
            OnGetDamage?.Invoke((CurrentHealth, MaxHealth));
        }
        OnChangeHealth?.Invoke((CurrentHealth, MaxHealth));
        if (CurrentHealth == 0)
        {
            Death();
        }
    }

    public void Death()
    {
        IsDead = true;
        OnDeath?.Invoke();
    }
}

using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{

    public int maxHealth = 100;
    public int currentHealth;

    public int attack = 0;
    public int fatigue = 0;
    public float infection = 0.0f;

    public event Action<float> OnHealthChanged;

    void Awake()
    {
        SetHealth(maxHealth);
    }

    public void TakeDamage(int amount)
    {
        SetHealth(currentHealth - amount);
    }

    public void Heal(int amount)
    {
        SetHealth(currentHealth + amount);
    }

    void SetHealth(int value)
    {
        currentHealth = value;

        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth > maxHealth) currentHealth = maxHealth;

        UpdateHealthBar();

        if (currentHealth == 0) Die();
    }


    void Die()
    {
        Debug.Log("ded");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
        }
    }

    private void UpdateHealthBar()
    {
        OnHealthChanged?.Invoke((float)currentHealth / maxHealth);
    }
}

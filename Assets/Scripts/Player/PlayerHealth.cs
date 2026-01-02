using UnityEngine;
using System;

public class PlayerHealth : Entity
{
    public int attack = 0;
    public int fatigue = 0;
    public float infection = 0.0f;

    public event Action<float> OnHealthChanged;

    void Awake()
    {
        maxHealth = 100;
        SetHealth(maxHealth);
    }


    private void Update()
    {
        UpdateHealthBar();
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

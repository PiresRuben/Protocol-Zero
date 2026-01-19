using UnityEngine;
using System;
using UnityEngine.UI;

public class PlayerHealth : Entity
{
    public int attack = 0;
    public int fatigue = 0;

    public event Action<float> OnHealthChanged;

    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private Slider infectionBar;

    void Awake()
    {
        maxHealth = 100;
        maxInfection = 100;
        SetHealth(maxHealth);
        SetInfection(0);
    }


    private void Update()
    {
        UpdateHealthBar();
        healthBar.value = currentHealth;
        infectionBar.value = currentInfection;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            TakeInfection(5);
        }
        Debug.Log("Health: " + currentHealth + " Infection: " + currentInfection);

    }

    private void UpdateHealthBar()
    {
        OnHealthChanged?.Invoke((float)currentHealth / maxHealth);
    }
}

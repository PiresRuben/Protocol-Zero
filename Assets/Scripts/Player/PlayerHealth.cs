using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class PlayerHealth : Entity
{
    public event Action<float> OnHealthChanged;

    [Header("UI References")]
    [SerializeField] private Slider healthBar;
    [SerializeField] private Slider infectionBar;

    protected override void Awake()
    {
        base.Awake();
        UpdateUI();
    }

    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount);
        UpdateUI();
    }

    public override void TakeInfection(int amount)
    {
        base.TakeInfection(amount);
        UpdateUI();
    }
    public override void Heal(int amount)
    {
        base.Heal(amount);
        UpdateUI();
    }

    public override void CureInfection(int amount)
    {
        base.CureInfection(amount);
        UpdateUI();
    }

    void UpdateUI()
    {
        if (healthBar != null)
            healthBar.value = currentHealth;
        if (infectionBar != null)
            infectionBar.value = currentInfection;

        OnHealthChanged?.Invoke((float)currentHealth / maxHealth);
    }
    protected override void Die()
    {
        Debug.Log("GAME OVER - Le joueur est mort");
        SceneManager.LoadScene(2);
    }
}
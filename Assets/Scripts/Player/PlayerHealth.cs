using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : Entity
{
    public event Action<float> OnHealthChanged;

    [Header("UI References")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image infectionBar;

    protected override void Awake()
    {
        base.Awake();
        UpdateUI();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            TakeInfection(5);
        }
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

    void UpdateUI()
    {
        if (healthBar != null)
            healthBar.fillAmount = (float)currentHealth / maxHealth;

        if (infectionBar != null)
            infectionBar.fillAmount = (float)currentInfection / maxInfection;

        OnHealthChanged?.Invoke((float)currentHealth / maxHealth);
    }
    protected override void Die()
    {
        Debug.Log("GAME OVER - Le joueur est mort");
    }
}
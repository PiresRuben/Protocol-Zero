using UnityEngine;
using UnityEngine.UI;
using System;

public class PlayerHealth : Entity
{
    public event Action<float> OnHealthChanged;

    [Header("UI References")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Image infectionBar;

    // On utilise override pour ajouter notre logique au Start du parent
    protected override void Awake()
    {
        base.Awake(); // Appelle le Awake de Entity (initialise la vie)
        UpdateUI();   // Met à jour l'UI au lancement
    }

    private void Update()
    {
        // Test inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(10);
            TakeInfection(5);
        }
    }

    // On modifie la prise de dégâts pour ajouter l'UI
    public override void TakeDamage(int amount)
    {
        base.TakeDamage(amount); // Fais les calculs de vie du parent
        UpdateUI();              // Puis mets à jour l'UI
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

    // Le joueur a peut-être une mort spéciale (Game Over screen)
    protected override void Die()
    {
        Debug.Log("GAME OVER - Le joueur est mort");
        // Ici tu appelleras ton GameManager pour relancer le niveau
    }
}
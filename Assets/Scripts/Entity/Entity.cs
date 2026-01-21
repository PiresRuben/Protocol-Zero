using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Entity Stats")]
    public int maxHealth = 100;
    public int maxInfection = 100;

    public int currentHealth { get; protected set; }
    public int currentInfection { get; protected set; }

    protected virtual void Awake()
    {
        currentHealth = maxHealth;
        currentInfection = 0;
    }

    public virtual void TakeDamage(int amount)
    {
        SetHealth(currentHealth - amount);
    }

    public virtual void Heal(int amount)
    {
        SetHealth(currentHealth + amount);
    }

    public virtual void TakeInfection(int amount)
    {
        SetInfection(currentInfection + amount);
    }

    public virtual void CureInfection(int amount)
    {
        SetInfection(currentInfection - amount);
    }

    protected void SetHealth(int value)
    {
        currentHealth = Mathf.Clamp(value, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected void SetInfection(int value)
    {
        currentInfection = Mathf.Clamp(value, 0, maxInfection);

        if (currentInfection >= maxInfection)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " est mort.");
        // Par défaut on détruit l'objet, mais l'ennemi changera ça
        // Destroy(gameObject); 
    }
}
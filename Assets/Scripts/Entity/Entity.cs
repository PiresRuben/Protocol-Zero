using UnityEngine;

public class Entity : MonoBehaviour 
{
    public int maxHealth = 100;

    public int maxInfection = 100;
    public int currentHealth { get; protected set; }
    public int currentInfection { get; protected set; }


    private void Die()
    {
        if (currentHealth <= 0) 
        {
            Debug.Log("ded");
            //DestroyImmediate(this);
        }
    }

    public void TakeDamage(int amount)
    {
        SetHealth(currentHealth - amount);
    }


    protected void Heal(int amount)
    {
        SetHealth(currentHealth + amount);
    }

    protected void SetHealth(int value)
    {
        currentHealth = value;

        if (currentHealth < 0) currentHealth = 0;
        if (currentHealth > maxHealth) currentHealth = maxHealth;



        if (currentHealth == 0) Die();
    }
    public void TakeInfection(int amount)
    {
        SetInfection(currentInfection + amount);
    }
    public void RemoveInfection(int amount)
    {
        SetInfection(currentInfection - amount);
    }

    protected void SetInfection(int value)
    {
        currentInfection = value;

        if (currentInfection < 0) currentInfection = 0;
        if (currentInfection > maxInfection) currentInfection = maxInfection;



        if (currentInfection == maxInfection) Die();
    }
}

using UnityEngine;

public class Entity : MonoBehaviour 
{
    public int maxHealth = 1;
    public int currentHealth { get; protected set; }


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
}

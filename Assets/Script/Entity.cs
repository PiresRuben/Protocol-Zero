using UnityEngine;

public class Entity : MonoBehaviour 
{
    private float currenthp = 1;


    private void Die()
    {
        if (currenthp <= 0) 
        {
            DestroyImmediate(this);
        }
    }

    private void TakeDamage(float damage)
    {
        currenthp -= damage;
        Die();
    }
}

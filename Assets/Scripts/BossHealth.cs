using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 1000;
    [SerializeField] private int currentHealth = 0;

    public float HealthFraction => (float)currentHealth / maxHealth;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public bool TakeDamage(int dmg)
    {
        if (currentHealth <= 0) return false;

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            print("Boss Defeated.");
        }

        return true;
    }
}

using UnityEngine;

public class BossHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 100;
    private int currentHealth = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int dmg)
    {
        if (currentHealth <= 0) return;

        currentHealth -= dmg;

        if (currentHealth <= 0)
        {
            print("Boss Defeated.");
        }
    }
}

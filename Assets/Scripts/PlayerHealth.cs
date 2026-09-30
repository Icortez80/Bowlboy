using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 3;
    private int currentHealth;
    private PlayerSpecialAttack special;

    private void Awake()
    {
        special = GetComponent<PlayerSpecialAttack>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;    
    }

    public bool TakeDamage(int damage)
    {
        //player already DED
        if (currentHealth <= 0) return false;
        //player currently using super
        if (special.IsSuperActive) return false;

        currentHealth -= damage;
        if(currentHealth <= 0)
        {
            print("Player defeated!");
        }

        return true;
    }
}

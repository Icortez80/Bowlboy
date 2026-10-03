using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] public int maxHealth = 3;
    [SerializeField] private float hitInvulnerabilityDuration = 1f;
    private float hitInvulnerabilityRemaining = 0f;
    [SerializeField]private int currentHealth;
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
    private void Update()
    {
        if(hitInvulnerabilityRemaining > 0)
        {
            hitInvulnerabilityRemaining -= Time.deltaTime;
        }
    }

    public bool TakeDamage(int damage)
    {
        //player already DED
        if (currentHealth <= 0) return false;
        //player currently using super
        if (special.IsSuperActive) return false;
        if (hitInvulnerabilityRemaining > 0) return false;
       
        currentHealth -= damage;
        //print("Player health -1");
        hitInvulnerabilityRemaining = hitInvulnerabilityDuration;
        if (currentHealth <= 0)
        {
            print("Player defeated!");
        }

        return true;
    }
}

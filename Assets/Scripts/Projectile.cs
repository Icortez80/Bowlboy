using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Team
    {
        Player,
        Enemy
    }

    [SerializeField] private Team team = Team.Player;
    [SerializeField] public float speed = 10f;
    [SerializeField] public int dmg = 1;
    [SerializeField] public float lifetime = 5f;

    private Rigidbody rb;
    private Vector3 direction = new Vector3(1f, 0f, 0f);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(team == Team.Player)
        {
            BossHealth boss = other.GetComponent<BossHealth>();
            if(boss != null)
            {
                boss.TakeDamage(dmg);
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection;
    }
}

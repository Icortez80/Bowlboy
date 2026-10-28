using UnityEngine;

public class Projectile : MonoBehaviour
{
    public enum Team
    {
        Player,
        Enemy
    }

    [SerializeField] private Team team = Team.Player;
    [SerializeField] public float speed = 14f;
    [SerializeField] public int dmg = 1;
    [SerializeField] public float lifetime = 5f;

    private Rigidbody rb;
    private Vector3 direction = new Vector3(1f, 0f, 0f);
    [SerializeField] private float rotationOffset = 0f;
    private PlayerEnergy energyOwner;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = direction.normalized * speed;
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //projectiles will be stopped by bounding walls/floor
        if (other.CompareTag("ProjectileBlocker"))
        {
            Destroy(gameObject);
            return;
        }

        if (team == Team.Player)
        {
            BossHealth boss = other.GetComponent<BossHealth>();
            if(boss != null)
            {
                //see if boss is dead and if projectile came from player
                bool result = boss.TakeDamage(dmg);
                if(result && energyOwner != null)
                {
                    energyOwner.AddNormalHit();
                }

                Destroy(gameObject);
            }

            PencilMinion minion = other.GetComponentInParent<PencilMinion>();

            if (minion != null)
            {
                minion.TakeDamage(dmg);
                Destroy(gameObject);
                return;
            }

        }
        else if(team == Team.Enemy)
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if(player != null)
            {
                player.TakeDamage(dmg);
                Destroy(gameObject);
            }
        }
    }

    public void SetDirection(Vector3 newDirection)
    {
        direction = newDirection.normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }

    public void SetEnergyOwner(PlayerEnergy owner)
    {
        energyOwner = owner;
    }
}

using UnityEngine;
using System.Collections.Generic;

public class PlayerBeam : MonoBehaviour
{
    [SerializeField] private int damagePerTick = 2;
    [SerializeField] private float tickInterval = 0.2f;

    private float tickTimer = 0f;
    private HashSet<Collider> overlappingColliders = new HashSet<Collider>();

    private void FixedUpdate()
    {
        tickTimer -= Time.fixedDeltaTime;

        if(tickTimer <= 0)
        {
            foreach (Collider target in overlappingColliders)
            {
                if (target == null) continue;

                BossHealth health = target.GetComponent<BossHealth>();

                if (health != null)
                {
                    health.TakeDamage(damagePerTick);
                }
            }

            tickTimer = tickInterval;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        BossHealth boss = other.GetComponent<BossHealth>();
        if(boss != null)
        {
            overlappingColliders.Add(other);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        overlappingColliders.Remove(other);
    }

}

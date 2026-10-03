using UnityEngine;

public class BossPencilNibAttack : MonoBehaviour
{
    [SerializeField] private Projectile nibPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int shotCount = 5;
    [SerializeField] private float spreadAngle = 40f;
    [SerializeField] private float shotInterval = 6f;

    private float shotTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shotTimer = shotInterval;
    }

    // Update is called once per frame
    void Update()
    {
        shotTimer -= Time.deltaTime;
        if(shotTimer <= 0)
        {
            FireSpread();
            shotTimer = shotInterval;
        }
    }

    private void FireSpread()
    {
        float angleStep = shotCount > 1 ? spreadAngle / (shotCount - 1) : 0f;
        for (int i = 0; i < shotCount; i++)
        {
            float angle = shotCount == 1 ? 0f : -spreadAngle * 0.5f + i * angleStep;
            Vector3 direction = Quaternion.Euler(0f, 0f, angle) * Vector3.left;
            Projectile shot = Instantiate(nibPrefab, firePoint.position, Quaternion.identity);
            shot.SetDirection(direction);
            //print(angle);

        }
    }
}

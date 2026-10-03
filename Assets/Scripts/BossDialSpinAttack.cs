using UnityEngine;

public class BossDialSpinAttack : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform upperFirePoint;
    [SerializeField] private Transform lowerFirePoint;
    [SerializeField] private int shotsPerBurst = 3;
    [SerializeField] private float shotSpacing = 0.3f;
    [SerializeField] private float burstCooldown = 2f;

    private Transform selectedFirePoint;
    private int shotsFired;
    private float timer;
    private bool isBursting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = burstCooldown;
        isBursting = false;

    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;

        if (!isBursting)
        {
            //not firing and waiting/wind up
            selectedFirePoint = Random.Range(0, 2) == 0 ? upperFirePoint : lowerFirePoint;
            shotsFired = 0;
            isBursting = true;
        }

        shotsFired += 1;
        Projectile shot = Instantiate(projectilePrefab, selectedFirePoint.position, Quaternion.identity);
        shot.SetDirection(Vector3.left);

        if(shotsFired >= shotsPerBurst)
        {
            isBursting = false;
            timer = burstCooldown;
        }
        else
        {
            timer = shotSpacing;
        }
        
        
    }
}

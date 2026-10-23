using System.Collections;
using UnityEngine;

public class BossDialSpinAttack : BossAttack
{
    [SerializeField] private Projectile projectilePrefab;
    [SerializeField] private Transform upperFirePoint;
    [SerializeField] private Transform lowerFirePoint;
    [SerializeField] private int shotsPerBurst = 3;
    [SerializeField] private float shotSpacing = 0.3f;
    [SerializeField] private float burstCooldown = 2f;
    [SerializeField] private BossAnimationController bossAnimation;

    private Transform selectedFirePoint;

    protected override IEnumerator ExecuteAttack()
    {
        bossAnimation.PlayDialSpin();
        // Wait before the burst begins.
        yield return new WaitForSeconds(burstCooldown);

        // Choose either the upper or lower fire point.
        selectedFirePoint =
            Random.Range(0, 2) == 0
                ? upperFirePoint
                : lowerFirePoint;

        // Fire the projectiles in sequence.
        for (int i = 0; i < shotsPerBurst; i++)
        {
            Projectile shot = Instantiate(
                projectilePrefab,
                selectedFirePoint.position,
                Quaternion.identity
            );

            shot.SetDirection(Vector3.left);

            // Wait between shots, but not after the final shot.
            if (i < shotsPerBurst - 1)
            {
                yield return new WaitForSeconds(shotSpacing);
            }
        }
    }
}
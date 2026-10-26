using System.Collections;
using UnityEngine;

public class BossPencilNibAttack : BossAttack
{
    [SerializeField] private Projectile nibPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private int shotCount = 5;
    [SerializeField] private float spreadAngle = 80f;
    [SerializeField] private float startupDelay = 1f;
    [SerializeField] private BossAnimationController bossAnimation;

    protected override IEnumerator ExecuteAttack()
    {
        bossAnimation.PlayNibBlast();

        yield return new WaitForSeconds(startupDelay);

        SoundController.Instance.Play(
            SoundController.SoundEffect.NibBlastCue
        );

        float angleStep = shotCount > 1
            ? spreadAngle / (shotCount - 1)
            : 0f;

        for (int i = 0; i < shotCount; i++)
        {
            float angle = shotCount == 1
                ? 0f
                : -spreadAngle * 0.5f + i * angleStep;

            Vector3 direction =
                Quaternion.Euler(0f, 0f, angle) * Vector3.left;

            Projectile shot = Instantiate(
                nibPrefab,
                firePoint.position,
                Quaternion.identity
            );

            shot.SetDirection(direction);
        }
    }
}
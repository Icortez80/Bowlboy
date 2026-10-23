using System.Collections;
using UnityEngine;

public class BossGraphiteBlastAttack : BossAttack
{
    [SerializeField] private GraphiteBlastZone upper;
    [SerializeField] private GraphiteBlastZone lower;

    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float activeDuration = 2f;
    [SerializeField] private float cooldownDuration = 6f;
    [SerializeField] private BossAnimationController bossAnimation;

    private GraphiteBlastZone selectedZone;

    protected override IEnumerator ExecuteAttack()
    {
        yield return new WaitForSeconds(cooldownDuration);

        selectedZone = Random.Range(0, 2) == 0
            ? upper
            : lower;

        if (selectedZone == upper)
        {
            bossAnimation.PlayGraphiteTop();
        }
        else
        {
            bossAnimation.PlayGraphiteBottom();
        }

        selectedZone.ShowWarning();

        yield return new WaitForSeconds(warningDuration);

        selectedZone.Activate();
        SoundController.Instance.PlayBeam();

        yield return new WaitForSeconds(activeDuration);

        SoundController.Instance.StopBeam();
        selectedZone.Hide();
    }

    public override void CancelAttack()
    {
        SoundController.Instance.StopBeam();

        upper.Hide();
        lower.Hide();

        selectedZone = null;

        base.CancelAttack();
    }
}
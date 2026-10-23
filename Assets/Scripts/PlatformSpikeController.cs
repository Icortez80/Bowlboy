using System.Collections;
using UnityEngine;

public class PlatformSpikeController : BossAttack
{
    [SerializeField] private PlatformSpike[] platformSpikes;
    [SerializeField] private float cooldownDuration = 3f;
    [SerializeField] private float warningDuration = 2f;
    [SerializeField] private float activeDuration = 3f;

    private PlatformSpike firstSelected;
    private PlatformSpike secondSelected;

    protected override IEnumerator ExecuteAttack()
    {
        yield return new WaitForSeconds(cooldownDuration);

        if (platformSpikes.Length < 2)
        {
            yield break;
        }

        int firstIndex = Random.Range(0, platformSpikes.Length);
        int secondIndex = firstIndex;

        while (secondIndex == firstIndex)
        {
            secondIndex = Random.Range(0, platformSpikes.Length);
        }

        firstSelected = platformSpikes[firstIndex];
        secondSelected = platformSpikes[secondIndex];

        firstSelected.ShowWarning();
        secondSelected.ShowWarning();

        SoundController.Instance.Play(
            SoundController.SoundEffect.SpikeRumble
        );

        yield return new WaitForSeconds(warningDuration);

        firstSelected.SetActive();
        secondSelected.SetActive();

        SoundController.Instance.Play(
            SoundController.SoundEffect.SpikeActivate
        );

        yield return new WaitForSeconds(activeDuration);

        firstSelected.Hide();
        secondSelected.Hide();

        firstSelected = null;
        secondSelected = null;
    }

    public override void CancelAttack()
    {
        foreach (PlatformSpike spike in platformSpikes)
        {
            spike.Hide();
        }

        firstSelected = null;
        secondSelected = null;

        base.CancelAttack();
    }
}
using UnityEngine;

public class PlatformSpikeController : MonoBehaviour
{
    private enum SpikeState
    {
        Cooldown,
        Warning,
        Active
    }

    [SerializeField] private PlatformSpike[] platformSpikes;
    [SerializeField] private float cooldownDuration = 1f;
    [SerializeField] private float warningDuration = 2f;
    [SerializeField] private float activeDuration = 3f;

    private PlatformSpike firstSelected;
    private PlatformSpike secondSelected;
    private SpikeState state;
    private float timer;
    private bool isRunning;

    private void Awake()
    {
        StopAttack();
    }

    private void Update()
    {
        if (!isRunning) return;

        timer -= Time.deltaTime;

        if (timer > 0f) return;

        if (state == SpikeState.Cooldown)
        {
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

            state = SpikeState.Warning;
            timer = warningDuration;
        }
        else if (state == SpikeState.Warning)
        {
            firstSelected.SetActive();
            secondSelected.SetActive();

            SoundController.Instance.Play(
                SoundController.SoundEffect.SpikeActivate
            );

            state = SpikeState.Active;
            timer = activeDuration;
        }
        else
        {
            firstSelected.ShowEmpty();
            secondSelected.ShowEmpty();

            firstSelected = null;
            secondSelected = null;

            state = SpikeState.Cooldown;
            timer = cooldownDuration;
        }
    }

    public void BeginAttack()
    {
        if (platformSpikes.Length < 2) return;

        isRunning = true;
        state = SpikeState.Cooldown;
        timer = cooldownDuration;

        foreach (PlatformSpike spike in platformSpikes)
        {
            spike.ShowEmpty();
        }
    }

    public void StopAttack()
    {
        isRunning = false;

        foreach (PlatformSpike spike in platformSpikes)
        {
            spike.Hide();
        }

        firstSelected = null;
        secondSelected = null;
        state = SpikeState.Cooldown;
        timer = cooldownDuration;
    }
}
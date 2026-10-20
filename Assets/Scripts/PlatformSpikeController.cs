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
    [SerializeField] private float warningDuration = 2f;
    [SerializeField] private float activeDuration = 3f;
    [SerializeField] private float cooldownDuration = 3f;

    private PlatformSpike firstSelected;
    private PlatformSpike secondSelected;
    private SpikeState state = SpikeState.Cooldown;
    private float timer;
    private int choice, choice2;

    private void Awake()
    {
        foreach (PlatformSpike spike in platformSpikes)
        {
            spike.Hide();
        }

        state = SpikeState.Cooldown;
        timer = 6f;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        
        if(timer <= 0)
        {
            if (state == SpikeState.Cooldown)
            {
                choice = Random.Range(0, platformSpikes.Length);

                choice2 = choice;
                while (choice2 == choice)
                {
                    choice2 = Random.Range(0, platformSpikes.Length);
                }

                firstSelected = platformSpikes[choice];
                secondSelected = platformSpikes[choice2];

                firstSelected.ShowWarning();
                secondSelected.ShowWarning();

                state = SpikeState.Warning;
                timer = warningDuration;
            }
            else if (state == SpikeState.Warning)
            {
                firstSelected.SetActive();
                secondSelected.SetActive();
                SoundController.Instance.Play(SoundController.SoundEffect.SpikeActivate);
                state = SpikeState.Active;
                timer = activeDuration;
            }
            else
            {
                firstSelected.ShowEmpty();
                secondSelected.ShowEmpty();
                state = SpikeState.Cooldown;
                timer = cooldownDuration;
            }
            
        }
    }
    public void StopAttack()
    {
        foreach (PlatformSpike spike in platformSpikes)
        {
            spike.Hide();
        }

        firstSelected = null;
        secondSelected = null;
        state = SpikeState.Cooldown;
        timer = cooldownDuration;
    }

    private void OnDisable()
    {
        StopAttack();
    }
}

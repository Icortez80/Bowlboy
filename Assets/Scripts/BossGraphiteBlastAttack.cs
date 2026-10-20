using UnityEngine;

public class BossGraphiteBlastAttack : MonoBehaviour
{
    public enum ZoneState
    {
        Cooldown,
        Warning,
        Active
    }

    [SerializeField] private GraphiteBlastZone upper;
    [SerializeField] private GraphiteBlastZone lower;
    [SerializeField] private float warningDuration = 1f;
    [SerializeField] private float activeDuration = 2f;
    [SerializeField] private float cooldownDuration = 6f;

    private GraphiteBlastZone selectedZone;
    private float timer = 0f;
    private ZoneState currentState = ZoneState.Cooldown;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = cooldownDuration;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer > 0) return;

        if (timer <= 0)
        {
            if(currentState == ZoneState.Cooldown)
            {
                currentState = ZoneState.Warning;
                //this line randomly chooses between the upper or lower zone
                selectedZone = Random.Range(0, 2) == 0 ? upper : lower;
                selectedZone.ShowWarning();
                timer = warningDuration;
            }else if(currentState == ZoneState.Warning)
            {
                currentState = ZoneState.Active;
                if(selectedZone == lower)
                {
                    SoundController.Instance.Play(SoundController.SoundEffect.GraphiteBlastBottom);
                }
                SoundController.Instance.PlayBeam();
                selectedZone.Activate();
                timer = activeDuration;
            }
            else
            {
                SoundController.Instance.StopBeam();
                currentState = ZoneState.Cooldown;
                selectedZone.Hide();
                timer = cooldownDuration;
            }
        }
    }

    public void StopAttack()
    {
        if (upper != null)
        {
            upper.Hide();
        }

        if (lower != null)
        {
            lower.Hide();
        }

        selectedZone = null;
        currentState = ZoneState.Cooldown;
        timer = cooldownDuration;
    }

    private void OnDisable()
    {
        StopAttack();
    }
}

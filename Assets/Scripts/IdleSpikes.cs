using UnityEngine;

public class IdleSpikes : MonoBehaviour
{
    private enum SpikeState
    {
        Cooldown,
        Warning,
        Active
    }

    [SerializeField] private SpriteRenderer[] visuals;
    [SerializeField] private Sprite warningSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Collider zoneCollider;
    [SerializeField] private int damage = 1;

    [SerializeField] private float warningDuration = 3f;

    private bool isActive;

    private SpikeState state = SpikeState.Cooldown;
    private float timer;

    private void Awake()
    {
        Hide();
        timer = warningDuration;
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (state == SpikeState.Cooldown)
            {
                state = SpikeState.Warning;
                ShowWarning();
                timer = warningDuration;
            }
            else if (state == SpikeState.Warning)
            {
                state = SpikeState.Active;
                SetActive();
            }
            else { return; }

        }
    }

    public void ShowWarning()
    {
        isActive = false;
        zoneCollider.enabled = false;

        foreach (SpriteRenderer visual in visuals)
        {
            visual.enabled = true;
            visual.sprite = warningSprite;
        }
    }

    public void SetActive()
    {
        isActive = true;
        zoneCollider.enabled = true;

        foreach (SpriteRenderer visual in visuals)
        {
            visual.enabled = true;
            visual.sprite = activeSprite;
        }
    }

    public void Hide()
    {
        isActive = false;
        zoneCollider.enabled = false;

        foreach (SpriteRenderer visual in visuals)
        {
            visual.enabled = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;

        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }
}
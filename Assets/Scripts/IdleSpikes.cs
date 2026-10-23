using System.Collections;
using UnityEngine;

public class IdleSpikes : BossAttack
{
    [SerializeField] private SpriteRenderer[] visuals;
    [SerializeField] private Sprite warningSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private Collider zoneCollider;
    [SerializeField] private int damage = 1;

    [SerializeField] private float warningDuration = 3f;
    [SerializeField] private float activeDuration = 2f;

    private bool isActive;
    private void Awake()
    {
        Hide();
    }

    protected override IEnumerator ExecuteAttack()
    {
        ShowWarning();

        SoundController.Instance.Play(
            SoundController.SoundEffect.SpikeRumble
        );

        yield return new WaitForSeconds(warningDuration);

        SetActive();

        SoundController.Instance.Play(
            SoundController.SoundEffect.SpikeActivate
        );

        yield return new WaitForSeconds(activeDuration);

        Hide();
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

        PlayerHealth player =
            other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    public override void CancelAttack()
    {
        Hide();
        base.CancelAttack();
    }
}
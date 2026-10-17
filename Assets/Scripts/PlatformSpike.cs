using UnityEngine;

public class PlatformSpike : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteVisual;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite warningSprite;
    [SerializeField] private Sprite activeSprite;
    [SerializeField] private int damage = 1;

    private bool isActive;

    public void ShowEmpty()
    {
        isActive = false;
        spriteVisual.enabled = true;
        spriteVisual.sprite = emptySprite;
    }

    public void ShowWarning()
    {
        isActive = false;
        spriteVisual.enabled = true;
        spriteVisual.sprite = warningSprite;
    }

    public void SetActive()
    {
        isActive = true;
        spriteVisual.enabled = true;
        spriteVisual.sprite = activeSprite;
    }

    public void Hide()
    {
        isActive = false;
        spriteVisual.enabled = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;

        PlayerHealth player = other.GetComponentInParent<PlayerHealth>();

        if (player != null)
        {
            //Debug.Log("Spike hit player");
            player.TakeDamage(damage);
        }
    }
}
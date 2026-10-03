using UnityEngine;

public class GraphiteBlastZone : MonoBehaviour
{
    [SerializeField] private Renderer zoneVisual;
    [SerializeField] private Material warningMaterial;
    [SerializeField] private Material activeMaterial;
    [SerializeField] private int damage = 1;


    private bool isActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        Hide();
    }

    public void ShowWarning()
    {
        isActive = false;
        zoneVisual.sharedMaterial = warningMaterial;
        zoneVisual.enabled = true;
    }

    public void Activate()
    {
        isActive = true;
        zoneVisual.sharedMaterial = activeMaterial;
        zoneVisual.enabled = true;
    }

    public void Hide()
    {
        isActive = false;
        zoneVisual.enabled = false;
    }

    //as long as the player is in the damage zone and not invincible
    //they will be hurt
    private void OnTriggerStay(Collider other)
    {
        if (!isActive) return;

        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if(player != null)
        {
            player.TakeDamage(damage);
        }
    }


}
